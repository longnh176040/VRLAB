using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Examples;
using IBM.Watsson.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XML.Parsing;
using UnityEngine.Networking;
using Boo.Lang;
using System;

public class SpeechSuggestion : MonoBehaviour
{
    [Header("Recording Tool")]
    public ExampleStreaming speechToTextConverter;

    [Header("Converting Tool")]
    public ExampleTextToSpeechV1 tTS;

    [Header("Speech Suggestion Canvas")]
    public GameObject canvas;

    [Header("Speech Suggestion Button")]
    public Button[] suggestionBtn;
    public Text scoreText, totalScoreText;
    public Image scoreBg, totalScoreBg;
    private Text[] suggestionText;

    private XMLParsingV1 xMLParsingV1;

    [HideInInspector]
    public static int sceneCount = 1;
    private static string SCENE_NUM = "Scene" + sceneCount.ToString();
    private int speechNum;
    private string[] suggestions;
    [HideInInspector]
    public List<float> score;
    [HideInInspector]
    public float totalScore;
    [HideInInspector]
    public string userAnswer;
    [HideInInspector]
    public bool startTalking, stopTalking, finishCheckingAnswer;

    void Awake()
    {
        Debug.Log("scene count " + SCENE_NUM);

        xMLParsingV1 = new XMLParsingV1();

        score = new List<float>();
        totalScore = 0;

        suggestions = new string[3];
        suggestionText = new Text[3];

        speechNum = 1;

        for (int i = 0; i < suggestionBtn.Length; i++)
            suggestionText[i] = suggestionBtn[i].GetComponentInChildren<Text>();

        canvas.SetActive(false);
        scoreBg.gameObject.SetActive(false);
        totalScoreBg.gameObject.SetActive(false);
    }


    void Update()
    {
        if (speechToTextConverter.finishRecordingAnswer && !finishCheckingAnswer)     //got the answer and start checking it 
        {
            CheckAnswer();
            speechNum++;                                                        //jump to the next speech;
            Debug.Log("speech Num " + speechNum);
            speechToTextConverter.finishRecordingAnswer = false;                //only accept one answer
            finishCheckingAnswer = true;
        }

        if (xMLParsingV1.endOfConversation)                                     //move to next scene
        {
            xMLParsingV1.endOfConversation = false;
            speechToTextConverter.StopRecording();

            //print total score of the conversation
            float sum = 0;
            for (int i = 0; i < score.Count; i++)
                sum += score[i];

            totalScore = sum / score.Count;
            totalScoreBg.gameObject.SetActive(true);
            totalScoreText.text = totalScore.ToString() + "%";

            StartCoroutine(DelayBeforeChangingScene(3f));
        }
    }

    public void SoundRecording()
    {
        if (speechToTextConverter.finishCreatingServer && startTalking)        //finish creating server, ready to record user's answer
        {
            speechToTextConverter.StartRecording();
            startTalking = false;
        }
    }

    //Need delay for npc to finishes their actions
    IEnumerator DelayBeforeChangingScene(float time) 
    {
        speechNum = 1;
        yield return new WaitForSeconds(time);
        sceneCount++;
        SCENE_NUM = "Scene" + sceneCount.ToString();
        xMLParsingV1.nullNum = 0;
        scoreBg.gameObject.SetActive(false);
        totalScoreBg.gameObject.SetActive(false);
        Debug.Log(SCENE_NUM);
    }

    public void PlayingSuggestionAudio(Button btn)
    {
        string suggestionText = btn.GetComponentInChildren<Text>().text;
        if (suggestionText != xMLParsingV1.NONE_SUGGESTION)
            Runnable.Run(tTS.ExampleSynthesize(suggestionText));
    }

    public void MakingSuggestion()
    {
        xMLParsingV1.XMLParsingUser(SCENE_NUM, "Speech" + speechNum.ToString(), suggestions);

        if (xMLParsingV1.endOfConversation)
            canvas.SetActive(false);
        else
        {
            for (int i = 0; i < suggestionText.Length; i++)
                suggestionText[i].text = suggestions[i];

            canvas.SetActive(true);
        }
    }

    public void ResetSuggestion() //call in npc script 
    {
        userAnswer = "";
        Debug.Log("reset null num");
        xMLParsingV1.nullNum = 0;
        finishCheckingAnswer = false;
        canvas.SetActive(false);
    }

    void CheckAnswer()
    {
        speechToTextConverter.StopRecording();
        Debug.Log("Answered, stop recording...");

        userAnswer = speechToTextConverter.userAnswer;
        Debug.Log("Your answer is " + userAnswer);

        var ansCheck = new DataCheck
        {
            sug_1 = suggestions[0],
            sug_2 = suggestions[1],
            sug_3 = suggestions[2],
            answer = userAnswer
        };

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(ansCheck);
        StartCoroutine(PostRequestCoroutine("http://fa3f446f2cea.ap.ngrok.io/check", json));
        Debug.Log(json);
    }

    public IEnumerator PostRequestCoroutine(string url, string json)
    {
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(json);

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();

        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest www =
            new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
            Debug.LogError(string.Format("{0}: {1}", www.url, www.error));
        else
        {
            Debug.Log("result: " + www.downloadHandler.text);
            scoreBg.gameObject.SetActive(true);
            float tmp = (float) Math.Round(float.Parse(www.downloadHandler.text), 2) * 100;
            scoreText.text = "Correct: " + tmp.ToString() + "%";
            score.Add(tmp);
        }
    }
}

class DataCheck
{
    public string sug_1;
    public string sug_2;
    public string sug_3;
    public string answer;
}

/*float AnswerAnalyzing()
    {
        float[] percents = new float[3]; //Store 3 values for later comparing
        float finalPercent;

        for (int i = 0; i < suggestions.Length; i++)
        {
            if (suggestions[i] != xMLParsingV1.NONE_SUGGESTION)
            {
                Debug.Log("Comparing with " + suggestions[i]);

                if (string.Compare(userAnswer, suggestions[i], true) == 0) //Correct
                {
                    Debug.Log("Correct");
                    return 100f; //finalPercent

                    //score += timer;
                    //score = Mathf.RoundToInt(score);
                }
                else
                {
                    float percent;
                    float error = 0;

                    char[] seperator = { ' ' };
                    string[] ans = userAnswer.Split(seperator);
                    string[] sug = suggestions[i].Split(seperator);

                    //Compare each words 
                    for (int j = 0; j < sug.Length; j++) //Cannot compare 2 strings with different length, fix later
                    {
                        Debug.Log("ans[j] " + ans[j]);
                        Debug.Log("sug[j] " + sug[j]);
                        if (string.Compare(ans[j], sug[j], true) != 0)
                            error++;
                        else continue;
                    }

                    Debug.Log("error " + error);

                    //and then calculate how much percent that is the same!
                    percent = ((sug.Length - error)/ sug.Length) * 100;
                    percents[i] = percent;             
                }
            }
            else percents[i] = 0;            
        }

        //Compare and get the correctest answer based on the largest percent
        finalPercent = percents[0];
        for(int i = 1; i<percents.Length; i++)
        {
            if (finalPercent < percents[i]) finalPercent = percents[i];
            else continue;
        }
        return finalPercent;
    }*/
