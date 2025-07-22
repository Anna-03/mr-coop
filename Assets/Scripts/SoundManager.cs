using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSourceVoice;
    public AudioSource audioSourceClick;
    public AudioSource audioSourceSuccess;

    public AudioClip click;
    public AudioClip correctMatch;
    public AudioClip incorrectMatch;
    public AudioClip success;
    public AudioClip background1;
    public AudioClip background2;
    public AudioClip background3;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayVoice(String clip)
    {

        AudioClip currentClip = audioSourceVoice.clip;
        switch (clip)
        {
            case "correct":
                audioSourceVoice.clip = correctMatch;
                break;

            case "incorrect":
                audioSourceVoice.clip = incorrectMatch;
                break;

            default:
                audioSourceVoice.clip = correctMatch;
                break;


        }
        if (!audioSourceVoice.isPlaying)
        {
            audioSourceVoice.Play();
            // audioSourceVoice.PlayOneShot(correctMatch);

            Debug.Log("nothing playing... clip played ");
            Debug.Log(audioSourceVoice.clip);
        }
        else if (currentClip != audioSourceVoice.clip)
        {
            audioSourceVoice.Stop();
            audioSourceVoice.Play();
            Debug.Log("stopped clip... clip played ");

        }


    }
}

