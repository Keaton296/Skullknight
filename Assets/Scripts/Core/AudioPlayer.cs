using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(int clipIndex = 0,bool loop = false)
    {
        if (clipIndex <= audioClips.Length - 1)
        {
            if (loop)
            {
                audioSource.clip = audioClips[clipIndex];
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClips[clipIndex]);
            }
        }
        else
        {
            throw new IndexOutOfRangeException();
        }
    }

    public void PlayRandom(bool loop = false)
    {
        int index = Random.Range(0, audioClips.Length);
        Play(index,loop);
    }

    public void Stop()
    {
        
    }
}
