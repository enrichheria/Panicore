using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RandomNameAndCountry.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    public CanvasGroup _CanvasGroup;

    public float fadeTime;
    public string _name;
    public TMP_Text nameText;
    public Image icon;
    private void Start()
    {
        nameText.text = RandomNameAndCountryPicker.Instance.GetRandomPlayerInfo().playerName;
       // nameText.text += ":";
        _CanvasGroup.DOFade(0f, fadeTime).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
