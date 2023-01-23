using System;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    [SerializeField] private Audio[] audios;
    [SerializeField] private AudioClip[] clips;
    public Dictionary<Audio, AudioClip> audioClips;// array of audio clips to play
    public AudioSource audioSource;
    public AudioSource loopingAudioSource;
    
    public enum Audio
    {
        MainMenu,
        StartGame,
        LoopGame,
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioClips = new Dictionary<Audio, AudioClip>();
        for (var i = 0; i < audios.Length; i++)
        {
            audioClips.Add(audios[i], clips[i]);
        }
        Debug.Log("Audio Clips set");
    }

    public void PlaySound(Audio clipName)
    {
        if (audioClips[clipName] == null) return;
        audioSource.PlayOneShot(audioClips[clipName]);
    }

    public void PlayLoop(Audio clipName)
    {
        if (audioClips[clipName] == null) return;
        loopingAudioSource.clip = audioClips[clipName];
        loopingAudioSource.loop = true;
        loopingAudioSource.Play();
    }
}
