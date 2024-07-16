using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    public CanvasGroup _Group;
    public bool isSplash;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!isSplash)
        {
            _Group.DOFade(0f, 0.5f);
            _Group.interactable = _Group.blocksRaycasts = false;
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                EventSender.instance.SendEventStartScene("main_menu");
            }
            else
            {
                EventSender.instance.SendEventStartScene("game_level");
            }
        }
    }

    public void LoadLevel(int id)
    {
        _Group.interactable = _Group.blocksRaycasts = true;
        _Group.DOFade(1f, 0.5f).OnComplete(() =>
        {
            StartCoroutine(WaitTime(id));
        });
    }

    IEnumerator WaitTime(int id)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(id);
    }
}
