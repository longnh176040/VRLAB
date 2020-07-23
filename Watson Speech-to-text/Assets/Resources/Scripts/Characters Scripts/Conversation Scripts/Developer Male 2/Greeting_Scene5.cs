using IBM.Cloud.SDK.Utilities;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Greeting_Scene5 : CharacterBase
{
    private static string SCENE_NUM = "Scene5";
    private static string TALKING_ANIMATION = "talking";
    private int totalSpeech;

    private void Start()
    {
        currentSpeech = 0;
        totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
        tTS.allisionVoice = "en-US_MichaelVoice";
    }

    private void Update()
    {
        //When player finishes his answer 
        if (speechSuggestion.finishCheckingAnswer && currentSpeech > 0 && currentSpeech < totalSpeech && startConv)
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(1f));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }

        /*if (currentSpeech >= totalSpeech)
            xMLParsingV1.endOfConversation = true;*/
    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        //tTS.service.SynthesizeUsingWebsockets(talkingText.text); //Convert text to speech
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));

        if (!xMLParsingV1.endOfConversation)
            speechSuggestion.startTalking = true;                //player's turn to speak      
    }

    void StartTalkingAnimation()
    {
        transform.LookAt(new Vector3(speechSuggestion.transform.position.x, 0, speechSuggestion.transform.position.z));
        anim.SetBool(TALKING_ANIMATION, true);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }
    void EndTalkingAnimation()
    {
        transform.Rotate(new Vector3(0, -150, 0), Space.Self);
        anim.SetBool(TALKING_ANIMATION, false);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;
            StartTalkingAnimation();
            SpeechHandler(talkingText, currentSpeech);
            //Wait till npc finishes talking and show suggestion
            if (SpeechSuggestion.sceneCount == 5)
                StartCoroutine(DurationPerSpeech(2f));
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
