using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InteractablePlace : MonoBehaviour
{
    public GameObject titpePlace;
    
    public void SetActiveInteractive(bool value)
    {
        titpePlace.SetActive(value);
    }

    public void Place()
    {
        PlayerController.instance.Place();
    }
    
}
