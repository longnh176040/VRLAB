using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_Learning1 : MonoBehaviour
{
    [SerializeField]
    private Transform learningObjects;

    [HideInInspector]
    public AudioClip[] audios;

    private PlayingRandomSound playingRandomSound;
    private ResultAudio resultAudio;

    public Image[] picking_Obj;

    public Sprite Accept, Deny, Default;

    private List<string> wrongAnswer;

    private bool startAssigning, checkRes;
    private bool startReading = true;

    private float score = 0;
    private int numCheck = 6;

    void Awake()
    {
        playingRandomSound = GameObject.FindGameObjectWithTag("Boss").GetComponent<PlayingRandomSound>();
        resultAudio = GetComponent<ResultAudio>();
        wrongAnswer = new List<string>();        

        audios = new AudioClip[numCheck];
    }

    void Update()
    {
        if (!startAssigning)
        {
            startAssigning = true;
            AssignAudio();
        }

        if (startReading)
        {
            StartReading();           
            startReading = false;
        }

        
        if ((PickingObject.isPicked || Timer.timer < 0f) && !checkRes)
        {
            CheckAnswer();
            StartCoroutine(DurationPerPicking(2f));
        } 
    }

    void AssignAudio() //add audio to the audio array
    {
        for (int i = 0; i < learningObjects.childCount - 1; i++)
            audios[i] = learningObjects.GetChild(i + 1).GetComponent<AudioSource>().clip; //need + 1 bc the 1st ele is an instanitiate pos
    }

    void StartReading()
    {
        if (numCheck > 0)
            playingRandomSound.PlayingRandom(audios);
    }

    private void CheckAnswer()
    {
        checkRes = true;

        for (int i = 0; i < learningObjects.childCount - 1; i++)
        {
            if (i == PlayingRandomSound.randNum)
            {
                picking_Obj[i].gameObject.GetComponent<Image>().sprite = Accept;        
                
                if (Timer.timer < 0)
                {
                    GameController.CheckDuplicateElement(wrongAnswer, learningObjects.GetChild(i).gameObject.name);
                    resultAudio.WrongAnnouncement();
                    playingRandomSound.playedAudio.Remove(PlayingRandomSound.randNum); //to check the wrong word once again
                    break;
                }

                if (learningObjects.GetChild(i + 1).gameObject.activeSelf == false)
                {
                    score += PickingObject.takeTime;
                    resultAudio.RightAnnouncement();
                    numCheck--;

                    if (numCheck == 0)
                        StartCoroutine(GameController.LoadScene(2));
                    else break;
                }
            }
            else if (learningObjects.GetChild(i + 1).gameObject.activeSelf == false) 
            {
                picking_Obj[i].gameObject.GetComponent<Image>().sprite = Deny;
                GameController.CheckDuplicateElement(wrongAnswer, learningObjects.GetChild(i).gameObject.name);
                playingRandomSound.playedAudio.Remove(PlayingRandomSound.randNum);
                resultAudio.WrongAnnouncement();
            }
        }        
    }

    IEnumerator DurationPerPicking(float time)
    {
        yield return new WaitForSeconds(time);
        startReading = true;
        score = Mathf.RoundToInt(score);
        //Debug.Log("score:" + score);
        PickingObject.takeTime = 0;
        Timer.timer = 15f;
        checkRes = false;
        PickingObject.isPicked = false;
        resultAudio.ResetText();
        ResetTurn();
    }

    void ResetTurn()
    {
        for (int i = 1; i < learningObjects.childCount; i++)
        {
            picking_Obj[i-1].gameObject.GetComponent<Image>().sprite = Default;
            learningObjects.GetChild(i).gameObject.SetActive(true);
        }
    }
}
