using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public AudioSource audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public void PlaySound(string fileName)
    {
        audioSource.clip = (AudioClip) Resources.Load("Sound/" + fileName);
        audioSource.Play();
    }
}
