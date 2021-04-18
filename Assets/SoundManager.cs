using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxsd;
    public AudioSource musicsd;
    public AudioSource UIsd;
    public AudioClip GameOver;

    public static SoundManager Instance;

    public AudioClip Breaksfx;

    private void Awake()
    {
        Instance = this;
    }

    public void PauseUnPauseMusic(bool pause = false)
    {
        if (pause)
        {
            musicsd.Pause();
        }
        else
        {
            musicsd.UnPause();
        }
    }
    public void PlaySoundEffect(AudioClip clip, bool voiceline = true)
    {
            sfxsd.pitch = Random.Range(0.9f, 1.1f);
            sfxsd.PlayOneShot(clip);

        
    }

    public void PlayUISoundEffect(AudioClip clip, bool voiceline = true)
    {
        UIsd.PlayOneShot(clip);
    }
}
