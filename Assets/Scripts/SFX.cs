using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    
    List<AudioSource> _AudioSources = new List<AudioSource>();
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _AudioSources.Add(child.GetComponent<AudioSource>());
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        for (int i = 0; i < _AudioSources.Count; i++)
        {
            if (_AudioSources[i].isPlaying == false)
            {
                _AudioSources[i].clip = clip;
                _AudioSources[i].Play();
                return;
            }
        }
        _AudioSources[0].clip = clip;
        _AudioSources[0].Play();
    }
}
