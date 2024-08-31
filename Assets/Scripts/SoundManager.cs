using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource audio;

    private void Awake()
    {
        instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound)
    {
        audio.PlayOneShot(_sound);
    }
}
