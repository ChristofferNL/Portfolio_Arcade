using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerGG : MonoBehaviour
{
    public static SoundManagerGG Instance;

    public AudioSource MusicSource;
    public AudioSource UnPitchedSource;
    public AudioSource PitchedSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip clip, bool isPitched)
    {
        if (!isPitched)
        {
            UnPitchedSource.PlayOneShot(clip);
        }
        else
        {
            PitchedSource.pitch = Random.Range(0.7f, 1.2f);
            PitchedSource.PlayOneShot(clip);
        }
    }
}
