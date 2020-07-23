using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Examples;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XML.Parsing;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Converting Tool")]
    public ExampleTextToSpeechV1 tTS;

    [Header("Player")]
    public SpeechSuggestion speechSuggestion;

    public GameObject background; //for 3D mode
    //public Image background;
    public Text talkingText;

    public XMLParsingV1 xMLParsingV1;
    protected Animator anim;

    [HideInInspector]
    public int currentSpeech;
    //[HideInInspector]
    public bool startConv;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        background.gameObject.SetActive(false);
        //talkingText.gameObject.SetActive(false);

        xMLParsingV1 = new XMLParsingV1();
    }

    public abstract void SpeechHandler(Text talkingText, int speechNum);
  
    public abstract void OnTriggerEnter(Collider other);

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            startConv = false;
    }

    public void PlayingDialogueAudio(Button btn)
    {
        string dialogueText = btn.GetComponentInChildren<Text>().text;
        //tTS.service.SynthesizeUsingWebsockets(dialogueText);
        Runnable.Run(tTS.ExampleSynthesize(talkingText.text));
    }

    public virtual IEnumerator DurationPerSpeech(float time)
    {
        yield return new WaitForSeconds(time);
        speechSuggestion.MakingSuggestion();
        currentSpeech++;                                        //jump to next speech
    }

    public virtual IEnumerator DurationPerSpeech(float time, Action animAct, Action animAct2)        //Code use to co-op anim and speech
    {
        //Change animAct to animAct2
        animAct();
        yield return new WaitForSeconds(time);
        animAct2();
        speechSuggestion.MakingSuggestion();
        currentSpeech++;                                        //jump to next speech
    }

}
