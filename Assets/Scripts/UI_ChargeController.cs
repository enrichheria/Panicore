using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChargeController : MonoBehaviour
{
    public Image _chargeImage;
    public List<GameObject> _chargeIndicators = new List<GameObject>();

    public GameObject _ChargeInfo;
    public bool isChargeEmpty, isChargeLast;

    public void SetActiveCharge(bool value)
    {
        _ChargeInfo.SetActive(true);
    }

    public void SetActiveChargeIndicator()
    {
        bool isConteins = false;
        for (int i = 0; i < _chargeIndicators.Count; i++)
        {
            if (_chargeIndicators[i].activeSelf)
            {
                _chargeIndicators[i].SetActive(false);
                if (i == _chargeIndicators.Count - 1)
                {
                    isChargeLast = true;
                }
                isConteins = true;
                break;
            }
        }

        if (!isConteins && isChargeLast)
        {
            isChargeEmpty = true;
            Debug.Log("AAAAA 2");
            _chargeImage.color = Color.red;
            InputController.instance.DisableButtonsNoEnergy();
        }
    }

    private void Update()
    {
        if (!isChargeEmpty && isChargeLast)
        {
            _chargeImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.5f));
        }
    }
}
