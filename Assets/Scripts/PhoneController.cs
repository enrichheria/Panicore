using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public GameObject _phone, _light, _streamOn,_streamOff, _noEnergy;
    public Transform activeState, unactiveState, streamState;
    public bool isNoEnergy;
    public bool isPhoneActive;
    public bool isLightActive;
    public bool isStreamActive;

    public Transform chat;
    public GameObject ChatItem;
    
    public float phoneWorkTime;
    public float timer, lastLime, checkTime;
    void Start()
    {
        StreamController.instance.SetPhone(this);
        PlayerController.instance._PhoneController = this;
        checkTime = phoneWorkTime / 5f;
    }

    private void Update()
    {
        if (!isNoEnergy)
        {
            if (isPhoneActive)
            {
                if (timer < phoneWorkTime + (checkTime))
                {
                    timer += 1f * Time.deltaTime;
                    if (lastLime + checkTime < timer)
                    {
                        lastLime = timer;
                        SetDownCharge();
                    }
                }
                else
                {
                    isNoEnergy = true;
                    DisablePhoneNoEnergy();
                    SetDownCharge();
                }
            }
        }
    }

    void SetDownCharge()
    {
        UIController.instance._UIChargeController.SetActiveChargeIndicator();
    }

    void DisablePhoneNoEnergy()
    {
        isLightActive = false;
        _light.SetActive(false);
        isStreamActive = false;
        _streamOn.SetActive(false);
        _streamOff.SetActive(false);
        _noEnergy.SetActive(true);
        StreamController.instance.SetActiveStream(false);
        transform.DOLocalMove(activeState.localPosition, 0.5f);
    }

    public void ActivePhone()
    {
        isPhoneActive = !isPhoneActive;
        if (isPhoneActive)
        {
            _phone.SetActive(isPhoneActive);
            transform.DOLocalRotate(activeState.localEulerAngles, 0.5f);
        }
        else
        {
            transform.DOLocalRotate(unactiveState.localEulerAngles, 0.5f).OnComplete(() =>
            {
                _phone.SetActive(isPhoneActive);
            });

            if (isLightActive)
            {
                ActiveLight();
            }

            if (isStreamActive)
            {
                ActiveStream();
            }
        }
    }
    public void ActiveLight()
    {
        if (!isNoEnergy)
        {
            isLightActive = !isLightActive;
            _light.SetActive(isLightActive);
            
        }
    }
    
    public void ActiveStream()
    {
        if (!isNoEnergy)
        {
            isStreamActive = !isStreamActive;
            _streamOn.SetActive(isStreamActive);
            _streamOff.SetActive(!isStreamActive);
            StreamController.instance.SetActiveStream(isStreamActive);
            if (isStreamActive)
            {
                transform.DOLocalMove(streamState.localPosition, 0.5f);
            }
            else
            {
                transform.DOLocalMove(activeState.localPosition, 0.5f);
            }
        }
    }

    public void SpawnChatItem(Color _color, Sprite _sprite)
    {
        if (isStreamActive)
        {
            GameObject item = Instantiate(ChatItem, chat);
            item.GetComponent<ChatItem>().icon.sprite = _sprite;
            item.GetComponent<ChatItem>().nameText.color = _color;
        }
    }
}
