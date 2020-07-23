using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IdleTalk_Scene16 : CharacterBase
{
    private static string SCENE_NUM = "Scene16";
    private int totalSpeech;
    private bool resetScene;

    void Start()
    {
        totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
        currentSpeech = 0;
        tTS.allisionVoice = "en-US_MichaelVoiceV3";
    }

    void Update()
    {
        if (speechSuggestion.finishCheckingAnswer && currentSpeech >= 0 && currentSpeech < totalSpeech && startConv)
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(2f));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }

        if (SpeechSuggestion.sceneCount == 22 && !resetScene)
        {
            SCENE_NUM = "Scene22";
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
            SpeechHandler(talkingText, currentSpeech);

            if (SpeechSuggestion.sceneCount == 16 || SpeechSuggestion.sceneCount == 22)
                StartCoroutine(DurationPerSpeech(2f)); 
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
