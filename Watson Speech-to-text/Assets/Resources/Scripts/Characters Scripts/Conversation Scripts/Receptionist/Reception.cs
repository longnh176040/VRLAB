using IBM.Cloud.SDK.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class Reception : CharacterBase
{
    private static string TYPING_ANIMATION = "searching";
    private static string TALKING_ANIMATION = "haveClient";
    private static string SCENE_NUM = "Scene1";
    private void Start()
    {
        tTS.allisionVoice = "en-US_LisaVoice";
        currentSpeech = 0;
    }

    private void Update()
    {
        if (speechSuggestion.finishCheckingAnswer && currentSpeech > 0 && startConv) //When player finishes his answer 
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            if (currentSpeech == 3)
                StartCoroutine(DurationPerSpeech(3f, StartSearchingAnim, StartReceptionAnim));
            else StartCoroutine(DurationPerSpeech(2f)); 
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }
    }

    private void StartReceptionAnim()
    {
        anim.SetBool(TALKING_ANIMATION, true);
        anim.SetBool(TYPING_ANIMATION, false);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }

    private void StartSearchingAnim()
    {
        anim.SetBool(TALKING_ANIMATION, false);
        anim.SetBool(TYPING_ANIMATION, true);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        //tTS.service.SynthesizeUsingWebsockets(talkingText.text); //Convert text to speech
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));
        if (!xMLParsingV1.endOfConversation)                     //Check if user's end of conv
            speechSuggestion.startTalking = true;                //player's turn to speak       
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;
            StartReceptionAnim();
            SpeechHandler(talkingText, currentSpeech);

            //Wait till npc finishes talking and show suggestion
            //Only show suggestions if scenecount = 1
            if (SpeechSuggestion.sceneCount == 1)           
                StartCoroutine(DurationPerSpeech(2f)); 
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = false;
            StartSearchingAnim();
        }
    }
}
