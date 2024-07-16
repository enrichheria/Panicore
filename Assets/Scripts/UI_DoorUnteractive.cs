using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DoorUnteractive : MonoBehaviour
{
    public GameObject TitleMain, NeedKey, Open, Close;

    public void ActiveNeedKey(bool value)
    {
        TitleMain.SetActive(value);
        NeedKey.SetActive(value);
    }
    
    public void ActiveOpen(bool value)
    {
        TitleMain.SetActive(value);
        Open.SetActive(value);
    }
    
    public void ActiveClose(bool value)
    {
        TitleMain.SetActive(value);
        Close.SetActive(value);
    }

    public void OpedCloseDoor()
    {
        if (PlayerController.instance._PlayerRayCaster.currentDoor != null)
        {
            PlayerController.instance._PlayerRayCaster.currentDoor.GetComponent<Door>().OpenCloseDoor();
            PlayerController.instance._PlayerRayCaster.currentDoor = null;
        }
    }

    public void CloseAll()
    {
        ActiveClose(false);
        ActiveOpen(false);
        ActiveNeedKey(false);
    }
}
