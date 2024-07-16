using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public CanvasGroup _Group;
    public List<GameObject> listTutorial = new List<GameObject>();
    private int currentPage;

    public GameObject btnClose;
    public void SetActiveTutorial(bool value)
    {
        _Group.interactable = _Group.blocksRaycasts = value;
        if (value)
        {
            if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
            {
                btnClose.SetActive(false);
                PlayerPrefs.SetInt("Tutorial", 1);
            }
            else
            {
                btnClose.SetActive(true);
            }
            currentPage = 0;

            SetActivePage();

            _Group.DOFade(1f, 0.5f);
            
            UIController.instance.SetActiveInputController(false);
            UIController.instance._Inventary.gameObject.SetActive(false);
        }
        else
        {
            _Group.DOFade(0f, 0.5f);
            
            UIController.instance.SetActiveInputController(true);
            UIController.instance._Inventary.gameObject.SetActive(true);
        }
    }

    public void Next()
    {
        currentPage++;
        SetActivePage();
    }

    void SetActivePage()
    {
        for (int i = 0; i < listTutorial.Count; i++)
        {
            listTutorial[i].SetActive(false);
        }
        listTutorial[currentPage].SetActive(true);
    }
}
