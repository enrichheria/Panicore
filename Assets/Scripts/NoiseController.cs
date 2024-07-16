using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    public static NoiseController instance;
    
    public Image lSlider, rSlider;

    public float currentFillValue;
    public float delayTimercalculate;
    private float timeToActive, timer;
    private bool isActive;

    private float timerCalculate;
    private bool isSneakersActive;
    void Awake()
    {
        instance = this;

        isSneakersActive = PlayerPrefs.GetInt("Sheakers", 0) == 1;
    }
    void Update()
    {
        if (isActive)
        {
            if (timer < timeToActive)
            {
                timer += 1f * Time.deltaTime;
            }
            else
            {
                isActive = false;
                timer = timeToActive = 0f;
                float tw = currentFillValue;
                DOTween.To(() => tw, x => tw = x, 0, 0.1f)
                    .OnUpdate(() =>
                    {
                        currentFillValue=tw;
                    });
            }
        }

        if (timerCalculate < delayTimercalculate)
        {
            timerCalculate += 1f * Time.deltaTime;
        }
        else
        {
            timerCalculate = 0f;
            SetSliderValue();
        }
    }

    public void AddNoise(float noiseAdd, float time)
    {
        if (isSneakersActive)
        {
           float Percentnoise = (noiseAdd / 100f) * 10f;
           noiseAdd -= Percentnoise;
        }

        timeToActive += time;
        if (noiseAdd > currentFillValue)
        {
            currentFillValue = noiseAdd;
        }

        isActive = true;
    }

    void SetSliderValue()
    {
        float val = Random.Range(currentFillValue - 0.1f, currentFillValue + 0.1f);
        lSlider.fillAmount = rSlider.fillAmount = val;
    }
}
