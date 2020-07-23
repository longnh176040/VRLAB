using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultAudio : MonoBehaviour
{
    public Text resultText;
    private ShakyText shakyText;

    private AudioClip rightAudio;
    private AudioClip wrongAudio;

    private AudioSource audioSource;

    void Awake()
    {
        rightAudio = Resources.Load<AudioClip>("Audio/Effect Audio/Right Answer");
        wrongAudio = Resources.Load<AudioClip>("Audio/Effect Audio/Wrong Answer");
        audioSource = GetComponent<AudioSource>();
        shakyText = resultText.GetComponent<ShakyText>();
        resultText.gameObject.SetActive(false);
    }

    public void RightAnnouncement()
    {
        audioSource.clip = rightAudio;
        resultText.gameObject.SetActive(true);
        audioSource.Play();
        resultText.color = Color.green;
        resultText.text = "Correct";
        shakyText.Shake();
    }

    public void WrongAnnouncement()
    {
        audioSource.clip = wrongAudio;
        resultText.gameObject.SetActive(true);
        audioSource.Play();
        resultText.color = Color.red;
        resultText.text = "Incorrect";
        shakyText.Shake();
    }

    public void ResetText()
    {
        resultText.gameObject.SetActive(false);
    }
}
