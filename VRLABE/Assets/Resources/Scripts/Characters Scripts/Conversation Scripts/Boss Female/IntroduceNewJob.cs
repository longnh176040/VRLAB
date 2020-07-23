using IBM.Cloud.SDK.Utilities;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IntroduceNewJob : CharacterBase
{
    private static string SCENE_NUM = "Scene3";
    private static string WALKING_ANIMATION = "walking";
    private static string TALKING_ANIMATION = "talking";
    private static string TYPING_ANIMATION = "typing";

    public Transform position1;
    public Transform position2;

    private int totalSpeech;
    private bool playerInArea;

    private void Start()
    {
        currentSpeech = 0;
        totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
        tTS.allisionVoice = "en-US_LisaVoice";
    }

    void Update()
    {
        //When player finishes his answer 
        if (speechSuggestion.finishCheckingAnswer && currentSpeech > 0 && startConv)
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(tTS.clipDuration));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }

        if (SpeechSuggestion.sceneCount == 3) 
            MovementHandler();

    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        //tTS.service.SynthesizeUsingWebsockets(talkingText.text); //Convert text to speech
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));

        if (!xMLParsingV1.endOfConversation)
            speechSuggestion.startTalking = true;                 //player's turn to speak             
        /*else if (xMLParsingV1.endOfConversation)
        {
            Debug.Log("can Move now");
            PlayerMovement.isMove = true;
        }*/
    }

    private void MovementHandler()
    {
        {
            if (playerInArea && !startConv && currentSpeech < totalSpeech)
            {
                //Debug.Log("distance sc " + Vector3.Distance(transform.position, position2.position));
                if (Vector3.Distance(transform.position, position2.position) > 0.5)
                {
                    transform.position = Vector3.Lerp(transform.position, position2.position, 0.05f);
                    transform.LookAt(position2.position);
                }
                else
                {
                    startConv = true;
                    transform.LookAt(new Vector3(speechSuggestion.transform.position.x, 0, speechSuggestion.transform.position.z));
                    //If player's in conversation, player's not allowed to move !
                    //PlayerMovement.isMove = false;
                    Debug.Log("cannot Move now");
                    StartTalkingAnimation();
                    SpeechHandler(talkingText, currentSpeech);
                    //Wait till npc finishes talking and show suggestion
                    StartCoroutine(DurationPerSpeech(2f));
                }
            }

            if (!playerInArea || (startConv && currentSpeech == totalSpeech))
            {
               // Debug.Log("distance ec " + Vector3.Distance(transform.position, position1.position));
                if (Vector3.Distance(transform.position, position1.position) > 0.5)
                {
                    transform.position = Vector3.Lerp(transform.position, position1.position, 0.05f);
                    transform.LookAt(position1.position);
                }
                else
                {
                    EndWalkingAnimation();
                    transform.rotation = Quaternion.Euler(new Vector3(0, 56f, 0));
                    startConv = false;
                }
            }
        }
    }

    void StartWalkingAnimation()
    {
        transform.LookAt(position2.transform);
        anim.SetBool(WALKING_ANIMATION, true);
        anim.SetBool(TYPING_ANIMATION, false);
    }
    
    public void StartTalkingAnimation()
    {
        anim.SetBool(WALKING_ANIMATION, false);
        anim.SetBool(TALKING_ANIMATION, true);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }

    void EndTalkingAnimation()
    {       
        anim.SetBool(TALKING_ANIMATION, false);
        anim.SetBool(WALKING_ANIMATION, true);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    void EndWalkingAnimation()
    {
        anim.SetBool(TYPING_ANIMATION, true);
        anim.SetBool(WALKING_ANIMATION, false);      
    }    

    public override void OnTriggerEnter(Collider other)
    {
        if (SpeechSuggestion.sceneCount == 3 && other.CompareTag("Player"))
        {
            StartWalkingAnimation();
            playerInArea = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            EndTalkingAnimation();
        }       
    }
}
