using IBM.Cloud.SDK.Utilities;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Interviewer : CharacterBase
{
    private static string SCENE_NUM = "Scene2";
    private int totalSpeech;
    private bool resetScene;

    public override void Awake()
    {
        base.Awake();
        totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
        currentSpeech = 0;
        tTS.allisionVoice = "en-US_MichaelVoiceV3";
    }
    
    void Update()
    {
        //When player finishes his answer 
        if (speechSuggestion.finishCheckingAnswer && currentSpeech >= 0 && currentSpeech < totalSpeech && startConv) 
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(2f));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }

        if (SpeechSuggestion.sceneCount == 12 && !resetScene)
        {
            SCENE_NUM = "Scene12";
            totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
            currentSpeech = 0;
            resetScene = true;
        }
        else if (SpeechSuggestion.sceneCount == 14 && resetScene)
        {
            SCENE_NUM = "Scene14";
            totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
            currentSpeech = 0;
            resetScene = false;
        }
        else if (SpeechSuggestion.sceneCount == 15 && !resetScene)
        {
            SCENE_NUM = "Scene15";
            totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
            currentSpeech = 0;
            resetScene = true;
        }
        else if (SpeechSuggestion.sceneCount == 17 && resetScene)
        {
            SCENE_NUM = "Scene17";
            totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
            currentSpeech = 0;
            resetScene = false;
        }
        else if (SpeechSuggestion.sceneCount == 21 && !resetScene)
        {
            SCENE_NUM = "Scene21";
            totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
            currentSpeech = 0;
            resetScene = true;
        }

    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));  //Convert text to speech
        if (!xMLParsingV1.endOfConversation)
            speechSuggestion.startTalking = true;                //player's turn to speak   
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;

            background.gameObject.SetActive(true);
            talkingText.gameObject.SetActive(true);

            //Wait till npc finishes talking and show suggestion
            if (SpeechSuggestion.sceneCount == 2)
            {
                SpeechHandler(talkingText, currentSpeech);
                StartCoroutine(DurationPerSpeech(2f));
            }
            else if (SpeechSuggestion.sceneCount == 12 || SpeechSuggestion.sceneCount == 14 || 
                SpeechSuggestion.sceneCount == 15 || SpeechSuggestion.sceneCount == 17 || 
                SpeechSuggestion.sceneCount == 21)
            {
                speechSuggestion.MakingSuggestion();
                speechSuggestion.startTalking = true;
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = false;
            background.gameObject.SetActive(false);
            talkingText.gameObject.SetActive(false);
        }
    }
}
