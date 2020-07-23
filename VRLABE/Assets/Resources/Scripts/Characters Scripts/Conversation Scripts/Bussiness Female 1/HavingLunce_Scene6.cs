using IBM.Cloud.SDK.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using XML.Parsing;

public class HavingLunce_Scene6 : CharacterBase
{
    private static string SCENE_NUM;
    private static string WALKING_ANIMATION = "walking";
    private static string TALKING_ANIMATION = "talking";
    private static string TYPING_ANIMATION = "typing";

    [SerializeField]
    private Transform [] positions;
    [SerializeField]
    private SpeechSuggestion player;
    [SerializeField]
    private GameObject playerSeat;

    private int currentPos = 0, totalSpeech;
    private bool resetScene;

    private void Start()
    {
        SCENE_NUM = "Scene6";
        totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
        currentSpeech = 0;
    }

    private void Update()
    {
        if (speechSuggestion.finishCheckingAnswer && currentSpeech > 0 && startConv) //When player finishes his answer 
        {
            SpeechHandler(talkingText, currentSpeech);          //Start next speech
            StartCoroutine(DurationPerSpeech(tTS.clipDuration));
            speechSuggestion.ResetSuggestion();                 //Reset pre answer of player
        }

        //need to check if player's staying at his working seat or not
        if ((SpeechSuggestion.sceneCount == 6 || SpeechSuggestion.sceneCount == 7) 
            && Vector3.Distance(player.transform.position, playerSeat.transform.position) < 3f)
        {
            MovementHandler();

            if (SpeechSuggestion.sceneCount == 7 && !resetScene) //MULTIPLE SCENES 6 + 7
            {
                SCENE_NUM = "Scene7";
                totalSpeech = xMLParsingV1.xNPCDoc.Descendants(SCENE_NUM).Descendants().Count();
                currentSpeech = 0;
                currentPos = 0;
                resetScene = true;
            }
        }        
    }

    public override void SpeechHandler(Text talkingText, int speechNum)
    {
        talkingText.text = xMLParsingV1.XMLParsingNPC(SCENE_NUM, "Speech" + speechNum.ToString()).Trim();
        //tTS.service.SynthesizeUsingWebsockets(talkingText.text); //Convert text to speech
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));

        if (!xMLParsingV1.endOfConversation)
            speechSuggestion.startTalking = true;                //player's turn to speak  
    }

    public void MovementHandler()
    {
        if (currentPos > positions.Length) currentPos = positions.Length;
        if (currentPos < 0) currentPos = 0;

        if (currentSpeech < totalSpeech)
        {
            if (currentPos < positions.Length)
            {
                if (Vector3.Distance(transform.position, positions[currentPos].position) > 0.5f)
                {
                    StartWalkingAnim();
                    transform.position = Vector3.Lerp(transform.position, positions[currentPos].position, 0.05f);
                    transform.LookAt(positions[currentPos].position);
                }
                else currentPos++;
            }
            else 
            {
                transform.LookAt(new Vector3(speechSuggestion.transform.position.x, 0, speechSuggestion.transform.position.z));
                //If player's in conversation, player's not allowed to move !
                //PlayerMovement.isMove = false;
                StartTalkingAnim(); 
            }
        }
        else if (startConv && currentSpeech == totalSpeech)
        {
            EndTalkingAnim();
            if (currentPos > 0)
            {
                if (Vector3.Distance(transform.position, positions[currentPos - 1].position) > 0.5)
                {
                    transform.position = Vector3.Lerp(transform.position, positions[currentPos - 1].position, 0.05f);
                    transform.LookAt(positions[currentPos - 1].position);
                }
                else currentPos--;

                if (currentPos == 0)
                {
                    EndWalkingAnim();
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    startConv = false;
                }
            }
        }
    }

    void StartTalkingAnim()
    {
        anim.SetBool(TALKING_ANIMATION, true);
        anim.SetBool(WALKING_ANIMATION, false);
        background.gameObject.SetActive(true);
        talkingText.gameObject.SetActive(true);
    }

    void EndTalkingAnim()
    {
        anim.SetBool(TALKING_ANIMATION, false);
        anim.SetBool(WALKING_ANIMATION, true);
        background.gameObject.SetActive(false);
        talkingText.gameObject.SetActive(false);
    }

    void StartWalkingAnim()
    {
        anim.SetBool(WALKING_ANIMATION, true);
        anim.SetBool(TYPING_ANIMATION, false);
    }

    void EndWalkingAnim()
    {
        anim.SetBool(TYPING_ANIMATION, true);
        anim.SetBool(WALKING_ANIMATION, false);
    }

    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startConv = true;
            SpeechHandler(talkingText, currentSpeech);
            //Wait till npc finishes talking and show suggestion
            StartCoroutine(DurationPerSpeech(2f));
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        //DO NOTHING
    }
}
