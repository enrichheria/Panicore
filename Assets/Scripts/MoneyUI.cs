using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text money;
    public Color _fadeColor;
    void Start()
    {
        money.DOColor(_fadeColor, 1.5f);
        transform.DOLocalMoveY(-400, 1.6f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    
}
