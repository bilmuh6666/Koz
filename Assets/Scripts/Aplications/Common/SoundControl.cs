using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioSource Source;
    void Start()
    {
        KozEventServices.SoundEffect.PlaySound += Play;
    }

    private void OnDestroy()
    {
        KozEventServices.SoundEffect.PlaySound -= Play;
    }

    public void Play()
    {
        Source.PlayOneShot(Source.clip);
    }
}
