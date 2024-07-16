using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    [Serializable]
    public class NeedId
    {
        public int ID, Count;
        public bool isConteins = false;
    }
    [Serializable]
    public class Receipt
    {
        public List<NeedId> needIDs = new List<NeedId>();
        public int CreatedID;
    }

    public AudioClip CraftSound;
    
    public List<Receipt> Receipts = new List<Receipt>();

    public List<GameObject> CraftButtons = new List<GameObject>();

    public void CheckCanCraft()
    {
        for (int i = 0; i < Receipts.Count; i++)
        {
            int isConteinsCount = 0;
            for (int k = 0; k < Receipts[i].needIDs.Count; k++)
            {
                for (int j = 0; j < UIController.instance._Inventary._CellsFull.Count; j++)
                {
                    if (UIController.instance._Inventary._CellsFull[j]._itemID == Receipts[i].needIDs[k].ID)
                    {
                        if (UIController.instance._Inventary._CellsFull[j]._itemCount >= Receipts[i].needIDs[k].Count)
                        {
                            isConteinsCount++;
                            Receipts[i].needIDs[k].isConteins = true;
                        }
                    }
                    else
                    {
                        Receipts[i].needIDs[k].isConteins = false;
                    }
                }
            }

            if (isConteinsCount == Receipts[i].needIDs.Count)
            {
                CraftButtons[i].SetActive(true);
            }
            else
            {
                CraftButtons[i].SetActive(false);
            }
        }

    }

    public void Craft(int id)
    {
        for (int k = 0; k < Receipts[id].needIDs.Count; k++)
        {
            for (int j = 0; j < UIController.instance._Inventary._CellsFull.Count; j++)
            {
                if (UIController.instance._Inventary._CellsFull[j]._itemID == Receipts[id].needIDs[k].ID)
                {
                    UIController.instance._Inventary._CellsFull[j].RemoveCount(Receipts[id].needIDs[k].Count);
                }
            }
        }
        EventSender.instance.SendEventCraftItem(ItemsInfo.instance.GetItemInfo(Receipts[id].CreatedID).itemName);
        //PlayerController.instance.DropItem(Receipts[id].CreatedID);
        UIController.instance._Inventary.PickUpItemID(Receipts[id].CreatedID);
        //NoiseController.instance.AddNoise(0.8f,1f);
        //UIController.instance.SetActiveInventary(false);
        SFX.instance.PlaySFX(CraftSound);
        CheckCanCraft();
    }
}
