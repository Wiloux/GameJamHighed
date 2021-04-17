using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxsd;
    public AudioSource musicsd;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void PlaySoundEffect(AudioClip clip)
    {
        sfxsd.pitch = Random.Range(0.9f, 1.1f);
        sfxsd.PlayOneShot(clip);
    }
}
