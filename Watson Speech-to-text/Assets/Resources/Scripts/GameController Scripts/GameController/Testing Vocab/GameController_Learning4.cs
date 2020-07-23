using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watsson.Examples;

public class GameController_Learning4 : MonoBehaviour
{
    [Header("Boss Male")]
    [Tooltip("The one who asking you so many questions")]
    [SerializeField]
    private TakingRandomObj takeItem;

    [Header("Boss's hand")]
    [SerializeField]
    private TakingItem objectName;

    [Header("GameController")]
    [Tooltip("GameController will render objects")]
    [SerializeField]
    private InstanitiateObjPos randomAnswers;

    [Header("Recording Tool")]
    public ExampleStreaming exampleStreaming;

    public Text answerText; 

    [Tooltip("Background text")]
    public Image background;

    private ResultAudio resultAudio;

    private List<string> wrongAnswer;

    private string userAnswer;

    private bool startCheckingAnswer, startRecording, timeUp, finishChecking;

    private float score = 0, timer;

    void Awake()
    {
        wrongAnswer = new List<string>();
        resultAudio = GetComponent<ResultAudio>();

        answerText.gameObject.SetActive(false);
        background.gameObject.SetActive(false);

        startRecording = true;
    }

    private void Start()
    {
        randomAnswers.RenderingPickedList(GameController.learning_Obj);
    }

    void Update()
    {
        if (Timer.timer < 0 && !exampleStreaming.finishRecordingAnswer && !timeUp)           //time up but no answer
        {
            if (Timer.timer < 0)
            {
                GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);    //assume that the answer is wrong
                resultAudio.WrongAnnouncement();
            }
            finishChecking = true;
            timeUp = true;
        }

        else if (exampleStreaming.finishRecordingAnswer && !startCheckingAnswer)     //got the answer and start checking it 
        {
            if (Timer.timer < 0)
            {
                GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);   //wrongAnswer.Add(objectName.child.name);
                resultAudio.WrongAnnouncement();
            }
            else if (Timer.timer > 0)
                timer = Timer.timer;

            CheckAnswer();
            exampleStreaming.finishRecordingAnswer = false;                     //only accept one answer
            startCheckingAnswer = true;
        }

        if (finishChecking)
        {
            takeItem.PuttingDown();
            finishChecking = false;
        }

        if (objectName.haveChild && exampleStreaming.finishCreatingServer && startRecording)        //finish creating server, ready to record user's answer
        {
            answerText.gameObject.SetActive(true);                              //load user answer
            background.gameObject.SetActive(true);
            exampleStreaming.StartRecording();
            startRecording = false;
        }

        if (!objectName.haveChild && !startRecording)                              //reset turn and delete old user's answer
        {
            Timer.timer = 15f;
            resultAudio.ResetText();
            answerText.gameObject.SetActive(false);                             
            background.gameObject.SetActive(false);
            startCheckingAnswer = false;
            startRecording = true;
            timeUp = false;                                                 //finsh checking
            userAnswer = "";
        }

        answerText.text = userAnswer;
    }

    public void CheckAnswer()                               
    {
        exampleStreaming.StopRecording();
        Debug.Log("Answered, stop recording...");

        userAnswer = exampleStreaming.userAnswer;
        Debug.Log("Your answer is " + userAnswer);

        if (string.Compare(userAnswer, objectName.child.name, true) == 0)
        {
            resultAudio.RightAnnouncement();
            Debug.Log("Correct");
            score += timer;
            score = Mathf.RoundToInt(score);
        }
        else
        {
            resultAudio.WrongAnnouncement();
            Debug.Log("Wrong");
            GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);
        }

        finishChecking = true;
    }
}
