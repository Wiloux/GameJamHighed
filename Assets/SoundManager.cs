using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxsd;
    public AudioSource musicsd;

    public static SoundManager Instance;

    public AudioClip Breaksfx;

    private void Awake()
    {
        Instance = this;
    }
    public void PlaySoundEffect(AudioClip clip, bool voiceline = true)
    {
 sfxsd.pitch = Random.Range(0.9f, 1.1f);
            sfxsd.PlayOneShot(clip);

        
    }
}
