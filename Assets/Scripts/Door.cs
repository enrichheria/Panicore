using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isNeedKey;

    public Transform openDoor, closeDoor;

    public bool isDoorOpened;
   
    public void OpenCloseDoor()
    {
        if (isDoorOpened)
        {
            isDoorOpened = false;
            transform.DORotateQuaternion(closeDoor.rotation, 0.5f);
        }
        else
        {
            if (!isNeedKey)
            {
                isDoorOpened = true;
                transform.DORotateQuaternion(openDoor.rotation, 0.5f);
            }
            else
            {
                for (int j = 0; j < UIController.instance._Inventary._CellsFull.Count; j++)
                {
                    if (UIController.instance._Inventary._CellsFull[j]._itemID == 12)
                    {
                        if (UIController.instance._Inventary._CellsFull[j]._itemCount >= 1)
                        {
                            UIController.instance._Inventary._CellsFull[j].RemoveCount();
                           UnlockDoor();
                        }
                    }
                }
            }
        }
    }

    public void OpenCloseDoorMonster()
    {
        isNeedKey = false;
        if (isDoorOpened)
        {
            isDoorOpened = false;
            transform.DORotateQuaternion(closeDoor.rotation, 0.5f);
        }
        else
        {
            isDoorOpened = true;
            transform.DORotateQuaternion(openDoor.rotation, 0.5f);
        }
    }
    public void UnlockDoor()
    {
        isNeedKey = false;
        OpenCloseDoor();
    }
}
