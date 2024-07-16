using System;
using System.Collections;
using System.Collections.Generic;
using CMF;
using DG.Tweening;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    public CanvasGroup _canvasGroup;
    public VariableJoystick _Joystick;
    public FixedTouchField _TouchField;
    [HideInInspector]
    public CameraController _CameraController;
    [HideInInspector]
    public CharacterKeyboardInput _CharacterKeyboardInput;

    public List<ButtonPhone> _ButtonsPhone = new List<ButtonPhone>();
    private void Awake()
    {
        instance=this;
        _CameraController = FindObjectOfType<CameraController>();
        _CharacterKeyboardInput = FindObjectOfType<CharacterKeyboardInput>();
    }
    
    public void SetActive(bool value)
    {
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = value;
        if (value)
        {
            _canvasGroup.DOFade(1f, 0.5f);
        }
        else
        {
            _canvasGroup.DOFade(0f, 0.5f);
        }
    }

    public void JumbButton(bool value)
    {
        _CharacterKeyboardInput.Jump(value);
    }

    public void DisableButtonsNoEnergy()
    {
        _ButtonsPhone[1].gameObject.SetActive(false);
        _ButtonsPhone[2].gameObject.SetActive(false);
    }
    public void BtnPhoneInput(string type)
    {
        if (type == "phone")
        {
            if (PlayerController.instance.IsPhoneExist())
            {
                PlayerController.instance._PhoneController.ActivePhone();
                _ButtonsPhone[0].SetActive();
                if (StreamController.instance._currentPhone.isNoEnergy == false)
                {
                    if (_ButtonsPhone[0].isActive)
                    {
                        for (int i = 1; i < _ButtonsPhone.Count; i++)
                        {
                            _ButtonsPhone[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        for (int i = 1; i < _ButtonsPhone.Count; i++)
                        {
                            if (_ButtonsPhone[i].isActive)
                            {
                                _ButtonsPhone[i].SetActive();
                            }

                            _ButtonsPhone[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        if (type == "light")
        {
            if (PlayerController.instance.IsPhoneExist())
            {
                PlayerController.instance._PhoneController.ActiveLight();
                _ButtonsPhone[1].SetActive();
            }
        }
        if (type == "stream")
        {
            if (PlayerController.instance.IsPhoneExist())
            {
                PlayerController.instance._PhoneController.ActiveStream();
                _ButtonsPhone[2].SetActive();
            }
        }
    }
    
}
