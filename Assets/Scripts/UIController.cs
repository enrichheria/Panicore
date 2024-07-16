using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    public SettingsController _SettingsController;
    public Inventary _Inventary;
    public GameObject pickUpMassage, bagFullMassage;

    public UI_InteractablePlace _UIInteractablePlace;
    public UI_ChargeController _UIChargeController;
    public UI_DoorUnteractive _UIDoorUnteractive;

    public CanvasGroup DeadPanel, ShopPanel;
    private Image _blurShop;
    public GameObject btnPhone, titleExit, titleShop;

    public Transform moneyParent;
    public GameObject moneyPlus;
    public TMP_Text moneyText;
    public float money;

    public Tutorial _Tutorial;

    public GameObject titleRun;
    public TMP_Text deadTitcleContinue;
    private DateTime _startTime;

    public List<ShopSlot> _ShopSlots = new List<ShopSlot>();
    private void Awake()
    {
        instance = this;
        money = PlayerPrefs.GetFloat("Money", 100);
    }

    private void Start()
    {
        _blurShop = ShopPanel.GetComponent<Image>();
        SetActivePhoneButton();
        
        SetMoneyText();

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            _Tutorial.SetActiveTutorial(true);
        }

        _startTime = DateTime.Now;
    }

    public void CheckCostShop()
    {
        for (int i = 0; i < _ShopSlots.Count; i++)
        {
            _ShopSlots[i].CheckCost();
        }
    }
    public void SetActiveTitleExit(bool value)
    {
        if (titleExit.activeSelf && !value)
        {
            titleExit.SetActive(false);
        }
        if (!titleExit.activeSelf && value)
        {
            titleExit.SetActive(true);
        }
    }
    
    public void SetActiveTitleShop(bool value)
    {
        if (titleShop.activeSelf && !value)
        {
            titleShop.SetActive(false);
        }
        if (!titleShop.activeSelf && value)
        {
            titleShop.SetActive(true);
        }
    }

    public void SetActiveDeadPanel()
    {
        DeadPanel.DOFade(1f, 2f).OnComplete(() =>
        {
            DeadPanel.interactable = DeadPanel.blocksRaycasts = true;
            StartCoroutine(WaitDead());
        });
    }

    public void SetActivePanelShop(bool value)
    {
        SetActiveInputController(!value);
        ShopPanel.interactable = ShopPanel.blocksRaycasts = value;
        if (value)
        {
            ShopPanel.DOFade(1f, 0.5f);
            
            float tw = 0;
            DOTween.To(() => tw, x => tw = x, 5, 0.5f)
                .OnUpdate(() =>
                {
                    _blurShop.material.SetFloat("_Size",tw);
                });
        }
        else
        {
            ShopPanel.DOFade(0f, 0.5f);
            float tw = 5;
            DOTween.To(() => tw, x => tw = x, 0, 0.5f)
                .OnUpdate(() =>
                {
                    _blurShop.material.SetFloat("_Size",tw);
                });
        }
    }

    public void ExitDead()
    {
        _Inventary.SaveInventaryEmpty();
        SaveMoney();
        DateTime currentTime = DateTime.Now;
        TimeSpan lifeTime = currentTime - _startTime;
        Debug.Log("TIME SPAND-"+lifeTime.TotalSeconds);
        EventSender.instance.SendEventExitGame(true,(int)lifeTime.TotalSeconds);
        LevelLoader.instance.LoadLevel(1);
    }

    IEnumerator WaitDead()
    {
        int count = 3;
        while (count>0)
        {
            deadTitcleContinue.text = "Continue (" + count.ToString() + ")";
            yield return new WaitForSeconds(1f);
            count--;
        }

        deadTitcleContinue.text = "Continue";
        ExitDead();
    }
    
    public void ExitComplete()
    {
        SaveMoney();
        DateTime currentTime = DateTime.Now;
        TimeSpan lifeTime = currentTime - _startTime;
        Debug.Log("TIME SPAND-"+lifeTime.TotalSeconds);
        EventSender.instance.SendEventExitGame(false, (int)lifeTime.TotalSeconds);
        LevelLoader.instance.LoadLevel(1);
    }
    public void SetMoneyText()
    {
        moneyText.text = money.ToString("F2") + " $";
    }

    public void PlusMoney(float value)
    {
        money += value;
        SetMoneyText();
        GameObject go = Instantiate(moneyPlus, moneyParent);
        go.GetComponent<MoneyUI>().money.text = "+" + value.ToString("F2") + " $";
    }
    
    public void MinusMoney(float value)
    {
        if (value <= money)
        {
            money -= value;
            SetMoneyText();
        }
        PlayerPrefs.SetFloat("Money", money);
    }

    public void SaveMoney()
    {
        int viewers = PlayerPrefs.GetInt("viewers", 100);
        if (viewers < StreamController.instance.viewers)
        {
            PlayerPrefs.SetInt("Viewers", StreamController.instance.viewers);
        }
        PlayerPrefs.SetFloat("Money", money);
    }

    public void SetActivePhoneButton()
    {
        if (StreamController.instance.isActiveStreamScene==false)
        {
            _UIChargeController.SetActiveCharge(false);
            btnPhone.SetActive(false);
        }
        else
        {
            _UIChargeController.SetActiveCharge(true);
            btnPhone.SetActive(true);
        }
    }
    public void SetActiveInteractivePlace(bool value)
    {
        _Inventary._canvasGroup.interactable = !value;
        _UIInteractablePlace.SetActiveInteractive(value);
    }
    
    public void SetActivePickUp(bool v)
    {
        if (v && !pickUpMassage.activeSelf)
        {
            pickUpMassage.SetActive(v);
        }
        else if (!v && pickUpMassage.activeSelf)
        {
            pickUpMassage.SetActive(v);
        }
    }
    
    public void SetActiveBagFull(bool v)
    {
        if (v && !bagFullMassage.activeSelf)
        {
            bagFullMassage.SetActive(v);
        }
        else if (!v && bagFullMassage.activeSelf)
        {
            bagFullMassage.SetActive(v);
        }
    }

    public void SetActiveInputController(bool v)
    {
        InputController.instance.SetActive(v);
    }

    public void SendPickUp()
    {
        PlayerController.instance.PickUp();
    }

    public void SetActiveInventary(bool v)
    {
        SetActiveInputController(!v);
        PlayerController.instance._BackPackController.OpenInventary(v);
    }
    
    
}
