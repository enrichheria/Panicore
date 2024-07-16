using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BGMMainMenu : MonoBehaviour
{
    public AudioSource _bgmPlayer;

    public void ActiveSound(bool value)
    {
        if (value)
        {
            _bgmPlayer.volume = 0;
            _bgmPlayer.DOFade(0.1f, 1f);
        }
        else
        {
            _bgmPlayer.volume = 0.1f;
            _bgmPlayer.DOFade(0, 1f);
        }
    }
}
