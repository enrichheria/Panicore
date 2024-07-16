using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int id;
    public bool isMini;
    public ItemType _ItemType;
    public int _itemID = -1;
    public int _itemCount;
    public Image itemIcon;
    public GameObject countCircle, lockImage, selectedCell;
    public TMP_Text itemCountText;

    private bool isSelected;
    public void DropItem()
    {
        if (isMini && _itemID != -1)
        {
            PlayerController.instance.DropItem(_itemID);
        }
    }

    public void ActiveSelected()
    {
        if (_itemID != -1)
        {
            if (!isSelected)
            {
                UIController.instance._Inventary.ResetSelectedCell();
            }
            isSelected = !isSelected;
            selectedCell.SetActive(isSelected);
            UIController.instance._Inventary.SelectCell(isSelected, id);
        }
    }

    public void RemoveSelection()
    {
        isSelected = false;
        if (selectedCell != null && selectedCell.activeSelf)
        {
            selectedCell.SetActive(isSelected);
        }
    }

    public void SetLock(bool value)
    {
        lockImage.SetActive(value);
    }

    void ResetTextCount()
    {
        itemCountText.text = _itemCount.ToString();
        countCircle.SetActive(_itemCount >= 2);
    }

    
    public void SetItem(int itemID)
    {
        ItemsInfo.ItemInfo item = ItemsInfo.instance.GetItemInfo(itemID);
        _ItemType = item._ItemType;
        _itemID = item.itemID;
        _itemCount = 1;
        itemIcon.sprite = item._Icon;
        itemIcon.gameObject.SetActive(true);
    }

    public void LoadItem(int id, int count)
    {
        if (id != -1)
        {
            ItemsInfo.ItemInfo item = ItemsInfo.instance.GetItemInfo(id);
            _ItemType = item._ItemType;
            _itemID = item.itemID;
            _itemCount = count;
            itemIcon.sprite = item._Icon;
            itemIcon.gameObject.SetActive(true);
        }
    }
    public void CopyCell(Cell cellToCopy)
    {
        if (cellToCopy._itemID != -1)
        {
            _ItemType = cellToCopy._ItemType;
            _itemID = cellToCopy._itemID;
            _itemCount = cellToCopy._itemCount;
            itemIcon.sprite = ItemsInfo.instance.GetIcon(_itemID);
            itemIcon.gameObject.SetActive(true);
            ResetTextCount();
        }
        else
        {
            _itemID = -1;
            _itemCount = 0;
            itemIcon.gameObject.SetActive(false);
            ResetTextCount();
        }
    }

    public void AddCount()
    {
        _itemCount++;
        ResetTextCount();
    }
    public void RemoveCount()
    {
        _itemCount--;
        if (_itemCount == 0)
        {
            _itemID = -1;
            itemIcon.gameObject.SetActive(false);
        }

        ResetTextCount();
        
        UIController.instance._Inventary.RefreshMiniCell();
    }
    
    public void RemoveCount(int count)
    {
        if (count <= _itemCount)
        {
            _itemCount-=count;
            if (_itemCount == 0)
            {
                _itemID = -1;
                itemIcon.gameObject.SetActive(false);
            }

            ResetTextCount();

            UIController.instance._Inventary.RefreshMiniCell();
        }
    }

    public void DeleteCell()
    {
        _itemID = -1;
        _itemCount = 0;
        itemIcon.gameObject.SetActive(false);
        RemoveSelection();
        ResetTextCount();
    }
}
