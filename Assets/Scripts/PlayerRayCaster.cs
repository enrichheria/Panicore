using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerRayCaster : MonoBehaviour
{
    public LayerMask _itemsLayer, _interactiveLayer, _floorLayer, _monsterLayer;

    public float radiusSpherecast;

    public bool isActive, isPlaceInteractable;
    public Transform center;
    public Transform currentSelectable;
    public Transform pointRay;

    public Transform currentInteractable;
    private InteractablePlace _interactablePlace;

    [HideInInspector] public bool isRayCast,isActiveStreamScene;
    
    [HideInInspector] public StreamState streamState;

    public List<Item> _ItemsAround = new List<Item>();

    public Transform currentDoor;
    void Update()
    {
        if (isActive)
        {
            if (!isPlaceInteractable)
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(pointRay.position, pointRay.forward, 5.0f, _interactiveLayer);
                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        RaycastHit hit = hits[i];
                        if (hit.transform.tag == "Item")
                        {
                            SetSelectable(hit.transform);
                        }

                        if (hit.transform.tag == "Door")
                        {
                            if (currentDoor != hit.transform)
                            {
                                if (currentDoor == null)
                                {
                                    UIController.instance._UIDoorUnteractive.CloseAll();
                                }
                                currentDoor = hit.transform;
                                Door door = currentDoor.GetComponent<Door>();
                                if (door.isDoorOpened)
                                {
                                    UIController.instance._UIDoorUnteractive.ActiveClose(true);
                                }
                                else 
                                {
                                    if (door.isNeedKey)
                                    {
                                        UIController.instance._UIDoorUnteractive.ActiveNeedKey(true);
                                    }
                                    else
                                    {
                                        UIController.instance._UIDoorUnteractive.ActiveOpen(true);
                                    }
                                }
                            }
                        }
                        if (hit.transform.tag == "Exit")
                        {
                            UIController.instance.SetActiveTitleExit(true);
                        }
                        
                        if (hit.transform.tag == "Shop")
                        {
                            UIController.instance.SetActiveTitleShop(true);
                        }
                    }
                }
                else
                {
                    SetSelectable(null);
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(pointRay.position, pointRay.forward, out hit,
                        5f, _floorLayer))
                    {
                        currentInteractable.position = hit.point;
                        _interactablePlace.isRayCast = true;
                    }
                    else
                    {
                        currentInteractable.position = pointRay.position + pointRay.forward * 5f;
                        _interactablePlace.isRayCast = false;
                    }
                }
            }
        }
        else
        {
            if (isActiveStreamScene)
            {
                RaycastHit hit;
                if (Physics.Raycast(pointRay.position, pointRay.forward, out hit,
                    30f, _monsterLayer))
                {
                    if (streamState != StreamState.monster)
                    {
                        streamState = StreamState.monster;
                        StreamController.instance.SetState(streamState);
                    }
                }
                else
                {
                    if (streamState != StreamState.idle)
                    {
                        streamState = StreamState.idle;
                        StreamController.instance.SetState(streamState);
                    }
                }
            }
        }
    }

    public void SetSelectable(Transform selectable)
    {
        if (currentDoor != null)
        {
            UIController.instance._UIDoorUnteractive.CloseAll();
            currentDoor = null;
        }
        if (selectable != null)
        {
            if (currentSelectable != selectable)
            {
                currentSelectable = selectable;
                if (UIController.instance._Inventary.IsCanPickUpItem(currentSelectable.GetComponent<Item>()))
                {
                    UIController.instance.SetActiveBagFull(false);
                    UIController.instance.SetActivePickUp(true);
                }
                else
                {
                    UIController.instance.SetActivePickUp(false);
                    UIController.instance.SetActiveBagFull(true);
                }
            }
        }
        else
        {
            if (currentSelectable != null)
            {
                currentSelectable = null;
            }
            UIController.instance.SetActivePickUp(false);
            UIController.instance.SetActiveBagFull(false);
            UIController.instance.SetActiveTitleExit(false);
            UIController.instance.SetActiveTitleShop(false);
        }
    }

    public void SetActiveTrigger(bool value)
    {
        isActive = value;
        GetComponent<Collider>().enabled = value;
        for (int i = 0; i < _ItemsAround.Count; i++)
        {
            if (_ItemsAround[i] != null)
            {
                _ItemsAround[i].ActiveOutLine(false);
            }
        }
        _ItemsAround.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            _ItemsAround.Add(other.gameObject.GetComponent<Item>());
            other.gameObject.GetComponent<Item>().ActiveOutLine(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.ActiveOutLine(false);
            for (int i = 0; i < _ItemsAround.Count; i++)
            {
                if (_ItemsAround[i] == item)
                {
                    _ItemsAround.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void SetInteractable(Transform item)
    {
        SetSelectable(null);
        currentInteractable = item;
        currentInteractable.GetComponent<Item>().ActivePlaceObject(true);
        _interactablePlace = currentInteractable.GetComponent<Item>().PlaceObgect.GetComponent<InteractablePlace>();
        UIController.instance.SetActiveInteractivePlace(true);
        isPlaceInteractable = true;
    }

    public void PlaceInteractable()
    {
        if (_interactablePlace.isCanPlace)
        {
            currentInteractable.GetComponent<Item>().ActivePlaceObject(false);
            UIController.instance.SetActiveInteractivePlace(false);
            isPlaceInteractable = false;
        }
        else
        {
            PlayerController.instance.PickUp();
            UIController.instance.SetActiveInteractivePlace(false);
            isPlaceInteractable = false;
        }
    }

    public void Rotate()
    {
        _interactablePlace.Rotate();
    }
}
