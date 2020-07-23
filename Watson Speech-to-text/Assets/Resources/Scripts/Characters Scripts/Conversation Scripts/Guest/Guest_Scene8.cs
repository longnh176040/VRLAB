using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guest_Scene8 : CharacterBase
{
    private static string SCENE_NUM = "Scene8";
    private static string STAND_UP_ANIMATION = "standUp";
    private static string TALKING_ANIMATION = "talking";
    private static string TYPING_ANIMATION = "typing";

    private void Start()
    {
        currentSpeech = 0;
        tTS.allisionVoice = "en-US_MichaelVoice";
    }

    private void Update()
    {
        if (speechSuggestion.finishCheckingAnswer && currentSpeech >= 0 && startConv) //When player finishes his answer 
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(2f));
            StartTalkingAnimation();
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }
    }

    void StartStandUpAnimation()
    {
        anim.SetBool(STAND_UP_ANIMATION, true);
        anim.SetBool(TYPING_ANIMATION, false);
    }

    void StartTalkingAnimation()
    {
        anim.SetBool(TALKING_ANIMATION, true);
        anim.SetBool(STAND_UP_ANIMATION, false);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }

    void EndTalkingAnimation()
    {
        anim.SetBool(TYPING_ANIMATION, true);
        anim.SetBool(TALKING_ANIMATION, false);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        //tTS.service.SynthesizeUsingWebsockets(talkingText.text); //Convert text to speech
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));

        if (!xMLParsingV1.endOfConversation)
            speechSuggestion.startTalking = true;                //player's turn to speak          
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;
            StartStandUpAnimation();

            speechSuggestion.MakingSuggestion();
            speechSuggestion.startTalking = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = false;
            EndTalkingAnimation();
        }
    }
}
