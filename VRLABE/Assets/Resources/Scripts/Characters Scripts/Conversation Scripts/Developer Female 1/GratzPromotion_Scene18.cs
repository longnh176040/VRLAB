using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GratzPromotion_Scene18 : CharacterBase
{
    private static string TALKING_ANIMATION = "talking";
    private static string SCENE_NUM = "Scene18";

    void Start()
    {
        tTS.allisionVoice = "en-US_LisaVoice";
        currentSpeech = 0;
    }

    void Update()
    {
        if (speechSuggestion.finishCheckingAnswer && currentSpeech > 0 && startConv) //When player finishes his answer 
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(2f));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }
    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));
        if (!xMLParsingV1.endOfConversation)                     //Check if user's end of conv
            speechSuggestion.startTalking = true;                //player's turn to speak           
    }

    private void StartTalkingAnim()
    {
        anim.SetBool(TALKING_ANIMATION, true);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }

    private void StartTypingAnim()
    {
        anim.SetBool(TALKING_ANIMATION, false);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;
            StartTalkingAnim();
            SpeechHandler(talkingText, currentSpeech);

            //Wait till npc finishes talking and show suggestion
            //Only show suggestions if scenecount = 18
            if (SpeechSuggestion.sceneCount == 18)
                StartCoroutine(DurationPerSpeech(2f));
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = false;
            StartTypingAnim();
        }
    }
}
