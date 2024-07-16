using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPhone : MonoBehaviour
{
    public string btnType;
    public GameObject activeState;
    public bool isActive;
    public void SetActive()
    {
        isActive = !isActive;
        activeState.SetActive(isActive);
    }

    public void PressButton()
    {
        InputController.instance.BtnPhoneInput(btnType);
    }
}
