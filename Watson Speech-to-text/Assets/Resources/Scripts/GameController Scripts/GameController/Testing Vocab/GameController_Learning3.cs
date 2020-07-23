using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_Learning3 : MonoBehaviour
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

    public InputField answer_field;
    public Text answer_text;
    public Button check_button;
    private ResultAudio resultAudio;

    private List<string> wrongAnswer;

    private bool isClicked, resetAnswer, loadField;

    private float score = 0, timer;
    private int numCheck = 10;

    void Awake()
    {
        wrongAnswer = new List<string>();
        resultAudio = GetComponent<ResultAudio>();

        answer_field.gameObject.SetActive(false);
        check_button.gameObject.SetActive(false);
    }

    private void Start()
    {
        randomAnswers.RenderingPickedList(GameController.learning_Obj);
    }

    void Update()
    {
        if (Timer.timer < 0) //time up
        {
            isClicked = true;
        }

        if (Input.GetKeyDown(KeyCode.Return)) CheckAnswer();

        if (isClicked)
        {
            if (Timer.timer == 0) GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);
            takeItem.PuttingDown();

            if (Timer.timer > 0) timer = Timer.timer;
            isClicked = false;

            resetAnswer = false;
        }

        if (objectName.haveChild && !loadField)
        {
            answer_field.gameObject.SetActive(true);
            check_button.gameObject.SetActive(true);
            loadField = true;
        }

        if (!objectName.haveChild && !resetAnswer)          //reset turn
        {
            Timer.timer = 15f;
            resultAudio.ResetText();
            answer_field.gameObject.SetActive(false);
            check_button.gameObject.SetActive(false);
            loadField = false;
            resetAnswer = true;
        }
    }

    public void CheckAnswer()                               //use for button
    {
        isClicked = true;
        string answer = answer_text.text;

        if (string.Compare(answer, objectName.child.name, true) == 0)
        {
            resultAudio.RightAnnouncement();
            score += timer;
            score = Mathf.RoundToInt(score);

            numCheck--;
            if (numCheck == 0)
                StartCoroutine(GameController.LoadScene(4));
        }
        else
        {
            resultAudio.WrongAnnouncement();
            GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);
        }

        answer_field.text = "";
    }
}
