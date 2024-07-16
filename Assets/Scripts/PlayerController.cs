using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public CameraController _CameraController;
    public CharacterKeyboardInput _CharacterKeyboardInput;

    public PlayerRayCaster _PlayerRayCaster;
    
    public PhoneController _PhoneController;

    public BackPackController _BackPackController;

    public GameObject phoneStandart, phonePowerfull;
    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.GetInt("PhoneID", 0) == 0)
        {
            phoneStandart.SetActive(true);
        }
        else
        {
            phonePowerfull.SetActive(true);
        }
    }
    
    

    void Start()
    {
        InputController.instance._CameraController = _CameraController;
        InputController.instance._CharacterKeyboardInput = _CharacterKeyboardInput;

        if (StreamController.instance.isActiveStreamScene)
        {
            StartCoroutine(WaitPhone());
        }
    }

    IEnumerator WaitPhone()
    {
        yield return new WaitWhile(() => _PhoneController == null);
        InputController.instance.BtnPhoneInput("phone");
    }

    public void SetInteractivePlace(Transform item)
    {
        _PlayerRayCaster.SetInteractable(item);
    }

    public void Place()
    {
        _PlayerRayCaster.PlaceInteractable();
    }

    public void Rotate()
    {
        _PlayerRayCaster.Rotate();
    }
    
    public void PickUp()
    {
        if (_PlayerRayCaster.currentSelectable != null)
        {
            Transform item = _PlayerRayCaster.currentSelectable;
            item.parent = transform;
            item.GetComponent<Item>().Collect();
            UIController.instance._Inventary.PickUpItem(item.GetComponent<Item>());
            _PlayerRayCaster.currentSelectable = null;
            item.DOLocalMove(new Vector3(0, 1, 0), 0.5f).OnComplete(() =>
            {
                Destroy(item.gameObject);
            });
        }
        else
        if (_PlayerRayCaster.currentInteractable != null)
        {
            Transform item = _PlayerRayCaster.currentInteractable;
            item.parent = transform;
            item.GetComponent<Item>().Collect();
            UIController.instance._Inventary.PickUpItem(item.GetComponent<Item>());
            _PlayerRayCaster.currentInteractable = null;
            item.DOLocalMove(new Vector3(0, 1, 0), 0.5f).OnComplete(() =>
            {
                Destroy(item.gameObject);
            });
        }
    }

    public void DropItem(int itemID, bool isDropInBag = false)
    {
        GameObject item = Instantiate(ItemsInfo.instance.GetItemInfo(itemID)._prefab, _PlayerRayCaster.pointRay.position,
            Quaternion.identity);
        item.GetComponent<Item>().Drop(_PlayerRayCaster.pointRay.forward,isDropInBag);
        UIController.instance._Inventary.DropItem(itemID);
    }
    public bool IsPhoneExist()
    {
        return _PhoneController != null;
    }
}
