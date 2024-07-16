using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    public AudioSource _AudioSource;

    public List<AudioClip> Clips = new List<AudioClip>();

    public AudioClip monsterAngry;

    private bool isPlayAngry;
    float wait;
    bool check;
    void Start()
    {
        PlayRandom();
    }

    public void PlayAngry()
    {
        if (isPlayAngry == false)
        {
            _AudioSource.clip = monsterAngry;
            _AudioSource.Play();
            wait = monsterAngry.length;
            check =isPlayAngry = true;
        }
    }

    public void PlayRandom()
    {
        int id = Random.Range(0, Clips.Count);
        _AudioSource.clip = Clips[id];
        _AudioSource.Play();
        wait = Clips[id].length;
        check=true;
    }
    void Update()
    {
        if(check){
            wait-=Time.deltaTime;
        }
 
        if((wait<0f) && (check))
        {
            if (isPlayAngry)
            {
                isPlayAngry = false;
            }
            check=false;
            PlayRandom();
        }
    }
}
