using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInfo : MonoBehaviour
{
    public static ItemsInfo instance;

    [Serializable]
    public class ItemInfo
    {
        public int itemID;
        public string itemName;
        public ItemType _ItemType;
        public Sprite _Icon;
        public GameObject _prefab;
    }

    public List<ItemInfo> _ItemsInfo = new List<ItemInfo>();
    private void Awake()
    {
        instance = this;
    }

    public ItemInfo GetItemInfo(int itemID)
    {
        for (int i = 0; i < _ItemsInfo.Count; i++)
        {
            if (_ItemsInfo[i].itemID == itemID)
            {
                return _ItemsInfo[i];
            }
        }

        return null;
    }

    public Sprite GetIcon(int itemID)
    {
        for (int i = 0; i < _ItemsInfo.Count; i++)
        {
            if (_ItemsInfo[i].itemID == itemID)
            {
                return _ItemsInfo[i]._Icon;
            }
        }

        return null;
    }
}
