using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    public TMP_Text costText;

    public string itemType;
    public int cost;

    public GameObject Buy;

    public void CheckCost()
    {
        if (cost > UIController.instance.money)
        {
            Buy.SetActive(false);
        }
    }

    public void Start()
    {
        costText.text = cost.ToString() + " $";
        if (cost > UIController.instance.money)
        {
            Buy.SetActive(false);
        }
        if (itemType == "phone")
        {
            if (PlayerPrefs.GetInt("PhoneID", 0) == 1)
            {
                gameObject.SetActive(false);
            }
        }
        
        if (itemType == "sneakers")
        {
            if (PlayerPrefs.GetInt("Sheakers", 0) == 1)
            {
                gameObject.SetActive(false);
            }
        }
        if (itemType == "backpack")
        {
            if (PlayerPrefs.GetInt("BigBag", 0) == 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void BuyItem()
    {
        if (cost <= UIController.instance.money)
        {
            if (itemType == "phone")
            {
                PlayerPrefs.SetInt("PhoneID", 1);
                gameObject.SetActive(false);
                UIController.instance.MinusMoney(cost);
                EventSender.instance.SendEventBuyItem("phone");
            }

            if (itemType == "sneakers")
            {
                PlayerPrefs.SetInt("Sheakers",1);
                gameObject.SetActive(false);
                UIController.instance.MinusMoney(cost);
                EventSender.instance.SendEventBuyItem("sneakers");
            }
            if (itemType == "trap")
            {
                if (UIController.instance._Inventary.IsCanPickUpItemID(11))
                {
                    UIController.instance._Inventary.PickUpItemID(11);
                    UIController.instance.MinusMoney(cost);
                    EventSender.instance.SendEventBuyItem(ItemsInfo.instance.GetItemInfo(11).itemName);
                }
            }
            if (itemType == "mine")
            {
                if (UIController.instance._Inventary.IsCanPickUpItemID(10))
                {
                    UIController.instance._Inventary.PickUpItemID(10);
                    UIController.instance.MinusMoney(cost);
                    EventSender.instance.SendEventBuyItem(ItemsInfo.instance.GetItemInfo(10).itemName);
                }
            }
            if (itemType == "mineice")
            {
                if (UIController.instance._Inventary.IsCanPickUpItemID(9))
                {
                    UIController.instance._Inventary.PickUpItemID(9);
                    UIController.instance.MinusMoney(cost);
                    EventSender.instance.SendEventBuyItem(ItemsInfo.instance.GetItemInfo(9).itemName);
                }
            }
            if (itemType == "radio")
            {
                if (UIController.instance._Inventary.IsCanPickUpItemID(13))
                {
                    UIController.instance._Inventary.PickUpItemID(13);
                    UIController.instance.MinusMoney(cost);
                    EventSender.instance.SendEventBuyItem(ItemsInfo.instance.GetItemInfo(13).itemName);
                }
            }
            if (itemType == "blackhole")
            {
                if (UIController.instance._Inventary.IsCanPickUpItemID(14))
                {
                    UIController.instance._Inventary.PickUpItemID(14);
                    UIController.instance.MinusMoney(cost);
                    EventSender.instance.SendEventBuyItem(ItemsInfo.instance.GetItemInfo(14).itemName);
                }
            }
            if (itemType == "backpack")
            {
                PlayerPrefs.SetInt("BigBag",1);
                gameObject.SetActive(false);
                UIController.instance.MinusMoney(cost);
                UIController.instance._Inventary.Init();
                EventSender.instance.SendEventBuyItem("backpack");
            }
        }
        
        UIController.instance.CheckCostShop();
    }
}
