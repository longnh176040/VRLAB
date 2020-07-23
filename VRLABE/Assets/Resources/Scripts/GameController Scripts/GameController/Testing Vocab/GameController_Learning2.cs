using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController_Learning2 : MonoBehaviour
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

    [SerializeField]
    private XMLParsing xmlParsing;

    private Text[] answers_text;
    private Button[] answer_button;
    private ResultAudio resultAudio;

    public GameObject[] answers;
    public Transform answer_panel;

    private List<int> pickedAnswer;            //use for storing picked object's name 
    private List<string> wrongAnswer;

    private bool isClicked, loadedAnswer, resetAnswer, timeUp;

    private float score = 0, timer;
    private int numCheck = 10;

    void Awake()
    {
        resultAudio = GetComponent<ResultAudio>();

        pickedAnswer = new List<int>();
        wrongAnswer = new List<string>();

        answer_button = new Button[4];
        answers_text = new Text[4];

        answer_panel.gameObject.SetActive(false);
        for (int i = 0; i < answers.Length; i++)
        {
            answer_button[i] = answers[i].GetComponent<Button>();
            answers_text[i] = answers[i].GetComponentInChildren<Text>();
            answers[i].SetActive(false);
        }
        //ResetAnswer();
    }

    private void Start()
    {
        randomAnswers.RenderingPickedList(GameController.learning_Obj);
    }

    void Update()
    {
        if (Timer.timer < 0 && !timeUp) //time up
        {
            GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);
            isClicked = true;
            timeUp = true;
        }

        if (isClicked)
        {       
            if (Timer.timer > 0) 
                timer = Timer.timer;

            takeItem.PuttingDown();

            isClicked = false;
            resetAnswer = false;
        }

        if (objectName.haveChild && !loadedAnswer)
        {
            loadedAnswer = true;
            LoadAnswer();
        }

        if (!objectName.haveChild && !resetAnswer)
        {
            ResetAnswer();
            resetAnswer = true;
        }
    }

    public void ResetAnswer() //calling when object is put down (TakingRandomObj class)
    {
        answer_panel.gameObject.SetActive(false);
        resultAudio.ResetText();

        for (int i = 0; i < answers_text.Length; i++)
        {
            answers_text[i].text = "";
            answers[i].SetActive(false);
        }
        pickedAnswer.Clear();
        Timer.timer = 15f;
        loadedAnswer = false;
        timeUp = false;
    }

    public void LoadAnswer()
    {
        answer_panel.gameObject.SetActive(true);

        for (int i = 0; i < answers.Length; i++)       
            if (!answers[i].activeSelf) answers[i].SetActive(true);
        
        int randNum = Random.Range(0, answers_text.Length);
        Debug.Log("answer loaded num " + randNum);
        answers_text[randNum].text = objectName.child.name;

        string key = xmlParsing.dic.FirstOrDefault(x => x.Value == objectName.child.name).Key; //Find key of object in the dictionary
        Debug.Log("Key" + key);        
        pickedAnswer.Add(int.Parse(key)); //Need to add the appropriate id of child on Dictionary.                                   

        for (int i = 0; i < answers_text.Length; i++)
        {
            if (answers_text[i].text == "")
            {
                answers_text[i].text = randomAnswers.parsingXML.dic[InstanitiateObjPos.RandomingObject(pickedAnswer).ToString()];
            }
        }
    }

    public void CheckAnswer(GameObject button)
    {
        isClicked = true;

        string checking_answer = button.GetComponentInChildren<Text>().text;

        if (string.Compare(checking_answer, objectName.child.name, true) == 0)
        {
            resultAudio.RightAnnouncement();
            score += timer;
            score = Mathf.RoundToInt(score);

            numCheck--;
            if (numCheck == 0)
                StartCoroutine(GameController.LoadScene(3));

        }
        else
        {
            resultAudio.WrongAnnouncement();
            GameController.CheckDuplicateElement(wrongAnswer, objectName.child.name);
        }
    }

}
