using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public CanvasGroup _canvasGroup;
    
    public SwitchManager _SwitchVibration;
    public SliderManager _SliderSens,_SliderVol;

    [Range(3,30)]
    public float sensivity=10;

    private Image _blur;
    private void Awake()
    {
        AudioListener.volume = 0;
    }

    private IEnumerator Start()
    {
        _blur = GetComponent<Image>();
        yield return new WaitWhile(() => InputController.instance._CameraController == null);
        //ChangeSens();
        ChangeVolume();
    }


    public void SetActive(bool value)
    {
        _canvasGroup.interactable = _canvasGroup.blocksRaycasts = value;
        if (value)
        {
            _canvasGroup.DOFade(1f, 0.5f);
            
            float tw = 0;
            DOTween.To(() => tw, x => tw = x, 5, 0.5f)
                .OnUpdate(() =>
                {
                    _blur.material.SetFloat("_Size",tw);
                });
        }
        else
        {
            _canvasGroup.DOFade(0f, 0.5f);
            float tw = 5;
            DOTween.To(() => tw, x => tw = x, 0, 0.5f)
                .OnUpdate(() =>
                {
                    _blur.material.SetFloat("_Size",tw);
                });
        }
    }
    
    public void ChangeSens()
    {
        sensivity = Remap(_SliderSens.GetValue(), 0, 100, 3, 30);
        Debug.Log(sensivity);
        InputController.instance._CameraController.cameraSpeed = sensivity;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = Remap(_SliderVol.GetValue(),0,100,0f,1f);
    }
    
    public float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
