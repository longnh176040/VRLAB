using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingRandomSound : MonoBehaviour
{
    private AudioSource audioSource;
    public List<int> playedAudio;
    public static int randNum;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playedAudio = new List<int>();
    }

    public void PlayingRandom(AudioClip[] audios)
    {
        audioSource.clip = audios[CheckPlayedAudio(audios)];
        audioSource.Play();
    }

    public int CheckPlayedAudio(AudioClip[] audios)
    {
        randNum = Random.Range(0, audios.Length);

        if (playedAudio.Count == 0)
        {
            playedAudio.Add(randNum);
            return randNum;
        }
        else
        {
            
            if (playedAudio.Contains(randNum))
                return CheckPlayedAudio(audios);

            playedAudio.Add(randNum);
            return randNum;
        }
    }
}
