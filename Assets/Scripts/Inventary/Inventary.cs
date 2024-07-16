using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Inventary : MonoBehaviour
{
    public CanvasGroup _canvasGroup;
    public Image _blur;
    public GameObject btnDeleteSelected;
    public int fullCellCount;

    public Transform miniCellConteiner, fullCellConteiner;
    
    public List<Cell> _CellsMini = new List<Cell>();
    public List<Cell> _CellsFull = new List<Cell>();

    private int currentCellSelected=-1;

    public CraftController _CraftController;
    private void Start()
    {
        if (PlayerPrefs.GetInt("BigBag", 0) == 1)
        {
            fullCellCount = 9;
        }
        else
        {
            fullCellCount = 6;
        }

        foreach (Transform child in miniCellConteiner)
        {
            if (child.GetComponent<Cell>() != null)
            {
                _CellsMini.Add(child.GetComponent<Cell>());
            }
        }
        foreach (Transform child in fullCellConteiner)
        {
            _CellsFull.Add(child.GetComponent<Cell>());
        }
        
        Init();
    }

    public void SetActive(bool value)
    {
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = value;
        if (value)
        {
            ResetSelectedCell();
            _CraftController.CheckCanCraft();
            _canvasGroup.DOFade(1f, 0.5f);
            
            float tw = 0;
            DOTween.To(() => tw, x => tw = x, 5, 0.5f)
                .OnUpdate(() =>
                {
                    _blur.material.SetFloat("_Size",tw);
                });
        }
        else
        {
            _canvasGroup.DOFade(0f, 0.5f);
            float tw = 5;
            DOTween.To(() => tw, x => tw = x, 0, 0.5f)
                .OnUpdate(() =>
                {
                    _blur.material.SetFloat("_Size",tw);
                });
        }
    }

   public void ResetSelectedCell()
    {
        for (int i = 0; i < _CellsFull.Count; i++)
        {
            _CellsFull[i].RemoveSelection();
        }
        currentCellSelected = -1;
        btnDeleteSelected.SetActive(false);
    }

    public void SelectCell(bool isSelect, int cellID)
    {
        if (isSelect)
        {
            currentCellSelected = cellID;
            btnDeleteSelected.SetActive(true);
        }
    }

    public void DeleteSelected()
    {
        if (currentCellSelected != -1)
        {
            //_CellsFull[currentCellSelected].DeleteCell();
            //currentCellSelected = -1;
            //RefreshMiniCell();
            DropItemSelected();
        }
    }
    public void Init()
    {
        if (PlayerPrefs.GetInt("BigBag", 0) == 1)
        {
            fullCellCount = 9;
        }
        else
        {
            fullCellCount = 6;
        }
        for (int i = 0; i < _CellsMini.Count; i++)
        {
            if (i + 1 > fullCellCount)
            {
                _CellsMini[i].gameObject.SetActive(false);
            }
            else
            {
                _CellsMini[i].gameObject.SetActive(true);
            }
        }
        
        for (int i = 0; i < _CellsFull.Count; i++)
        {
            if (i + 1 > fullCellCount)
            {
                _CellsFull[i].SetLock(true);
            }
            else
            {
                _CellsFull[i].SetLock(false);
            }
        }

        LoadInventary();

    }

    public bool IsCanPickUpItem(Item _item)
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._itemID==_item.ID || _CellsFull[i]._itemID==-1)
            {
                return true;
            }
        }
        return false;
    }
    public void PickUpItem(Item _item)
    {
        int cellIDWithItem = GetCellWithItem(_item.ID);
        
        if (_item.Type == ItemType.interactable)
        {
            if (cellIDWithItem == -1)
            {
                if (isCanSetToMiniCell())
                {
                    ReplaceCell(GetCellToReplace(),_item.ID);
                }
                else
                {
                    SetItemToEmptyCell(GetEmptyCell(),_item.ID);
                }
            }
            else
            {
                AddCountItemsToCell(cellIDWithItem);
            }
        }
        else
        {
            if (cellIDWithItem == -1)
            {
                SetItemToEmptyCell(GetEmptyCell(),_item.ID);
            }
            else
            {
                AddCountItemsToCell(cellIDWithItem);
            }
        }
        
        RefreshMiniCell();
        EventSender.instance.SendEventCollectItem(ItemsInfo.instance.GetItemInfo(_item.ID).itemName);
    }
    
    public bool IsCanPickUpItemID(int _itemID)
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._itemID==_itemID || _CellsFull[i]._itemID==-1)
            {
                return true;
            }
        }
        return false;
    }
    public void PickUpItemID(int _itemID)
    {
        int cellIDWithItem = GetCellWithItem(_itemID);
        
        if (cellIDWithItem == -1)
        {
            if (isCanSetToMiniCell())
            {
                ReplaceCell(GetCellToReplace(),_itemID);
            }
            else
            {
                SetItemToEmptyCell(GetEmptyCell(),_itemID);
            }
        }
        else
        {
            AddCountItemsToCell(cellIDWithItem);
        }
        
        RefreshMiniCell();
        //EventSender.instance.SendEventCollectItem(ItemsInfo.instance.GetItemInfo(_itemID).itemName);
    }

    public void DropItem(int itemID)
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._itemID==itemID)
            {
                _CellsFull[i].RemoveCount();
                return;
            }
        }
    }
    
    public void DropItemCount(int itemID, int count)
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._itemID==itemID)
            {
                _CellsFull[i].RemoveCount(count);
                return;
            }
        }
    }
    public void DropItemSelected()
    {
        PlayerController.instance.DropItem(_CellsFull[currentCellSelected]._itemID,true);
        _CellsFull[currentCellSelected].RemoveCount();
        ResetSelectedCell();
    }
    int GetCellWithItem(int id)
    {
        for (int i = 0; i < _CellsFull.Count; i++)
        {
            if (_CellsFull[i]._itemID == id)
            {
                return i;
            }
        }
        return -1;
    }
    bool isCanSetToMiniCell()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._ItemType==ItemType.part)
            {
                return true;
            }
        }
        return false;
    }
    int GetCellToReplace()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._ItemType==ItemType.part)
            {
                return i;
            }
        }
        return -1;
    }
    int GetEmptyCell()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            if (_CellsFull[i]._itemID==-1)
            {
                return i;
            }
        }
        return -1;
    }
    public void ReplaceCell(int cellToReplace, int itemID)
    {
        if (cellToReplace != -1)
        {
            int emptyCell = GetEmptyCell();
            if (emptyCell != -1)
            {
                _CellsFull[emptyCell].CopyCell(_CellsFull[cellToReplace]);
                _CellsFull[cellToReplace].SetItem(itemID);
            }
        }
        else
        {
            Debug.Log("Err");
        }
    }
    public void SetItemToEmptyCell(int cellId, int itemID)
    {
        _CellsFull[cellId].SetItem(itemID);
    }

    public void AddCountItemsToCell(int cellId)
    {
        _CellsFull[cellId].AddCount();
    }

    public void RefreshMiniCell(bool save = true)
    {
        for (int i = 0; i < _CellsMini.Count; i++)
        {
            _CellsMini[i].CopyCell(_CellsFull[i]);
        }

        if (save)
        {
            SaveInventary();
        }
    }

    public void SaveInventary()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            PlayerPrefs.SetInt("Cell-"+i+"item-",_CellsFull[i]._itemID);
            PlayerPrefs.SetInt("Cell-"+i+"count-",_CellsFull[i]._itemCount);
        }
    }

    
    public void SaveInventaryEmpty()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            PlayerPrefs.SetInt("Cell-" + i + "item-", -1);
            PlayerPrefs.SetInt("Cell-" + i + "count-", 0);
            /*
            if (_CellsFull[i]._ItemType == ItemType.part)
            {
                PlayerPrefs.SetInt("Cell-" + i + "item-", -1);
                PlayerPrefs.SetInt("Cell-" + i + "count-", 0);
            }
            else
            {
                if (_CellsFull[i]._itemID >= 9)
                {
                    PlayerPrefs.SetInt("Cell-" + i + "item-", _CellsFull[i]._itemID);
                    PlayerPrefs.SetInt("Cell-" + i + "count-", _CellsFull[i]._itemCount);
                }
                else
                {
                    PlayerPrefs.SetInt("Cell-" + i + "item-", -1);
                    PlayerPrefs.SetInt("Cell-" + i + "count-", 0);
                }
            }*/
        }
    }
    
    public void SaveInventaryItemBuy()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
           
            if (_CellsFull[i]._ItemType == ItemType.part)
            {
                PlayerPrefs.SetInt("Cell-" + i + "item-", -1);
                PlayerPrefs.SetInt("Cell-" + i + "count-", 0);
            }
            else
            {
                if (_CellsFull[i]._itemID >= 9)
                {
                    PlayerPrefs.SetInt("Cell-" + i + "item-", _CellsFull[i]._itemID);
                    PlayerPrefs.SetInt("Cell-" + i + "count-", _CellsFull[i]._itemCount);
                }
                else
                {
                    PlayerPrefs.SetInt("Cell-" + i + "item-", -1);
                    PlayerPrefs.SetInt("Cell-" + i + "count-", 0);
                }
            }
        }
    }

    public void LoadInventary()
    {
        for (int i = 0; i < fullCellCount; i++)
        {
            _CellsFull[i].LoadItem(PlayerPrefs.GetInt("Cell-"+i+"item-",-1),PlayerPrefs.GetInt("Cell-"+i+"count-",0));
        }
        RefreshMiniCell(false);
    }
}
