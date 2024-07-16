using System;
using System.Collections;
using System.Collections.Generic;
using RandomNameAndCountry.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StreamController : MonoBehaviour
{
    public static StreamController instance;

    public PhoneController _currentPhone;
    

    public StreamState streamState;
    public StreamTextPhone _StreamTextPhone;
    public bool isStreamOn;
    public float minTime, maxTime;
    public float timePerChat;
    private float timer;
    
    public List<Color> nameColors = new List<Color>();
    public List<Sprite> idleIcons = new List<Sprite>();
    public List<Sprite> monsterIcons = new List<Sprite>();
    public List<Sprite> monsterIsStopedIcons = new List<Sprite>();

    public bool isActiveStreamScene;

    public int viewers = 100;
    public int viewersPlusPercent = 10;
    public int viewersMinusPercent = 5;
    public float moneyPlusPerViwer = 0.01f;
    
    private float timerSteam, lastCheckTime;
    
    void Awake()
    {
        instance = this;
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            isActiveStreamScene = true;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            PlayerController.instance._PlayerRayCaster.isActiveStreamScene = true;
            viewers = PlayerPrefs.GetInt("Viewers", 100);
        }
    }

    public void SetPhone(PhoneController _phoneController)
    {
        _currentPhone = _phoneController;
        timePerChat = Random.Range(minTime, maxTime);
    }

    public void SetActiveStream(bool value)
    {
        if (isActiveStreamScene)
        {
            isStreamOn = value;
            if (value)
            {
                EventSender.instance.SendEventStreamOn();
                UIController.instance.SetActivePickUp(false);
                UIController.instance.SetActiveBagFull(false);
                UIController.instance.SetActiveTitleExit(false);
                UIController.instance.SetActiveTitleShop(false);
            }
        }
        PlayerController.instance._PlayerRayCaster.SetActiveTrigger(!value);
    }

    public void SetState(StreamState _state)
    {
        streamState = _state;
    }

    void CalculateMoney()
    {
        if (streamState == StreamState.idle)
        {
            int countMinusViewers =(int) (((float)viewers / 100f) * (float)viewersMinusPercent);
            viewers -= countMinusViewers;
            if (viewers < 100)
            {
                viewers = 100;
            }
        }
        else
        {
            int countPlusViewers = (int) (((float)viewers / 100f) * (float)viewersPlusPercent);
            viewers += countPlusViewers;
        }

        if (viewers <= 0)
        {
            viewers = 0;
        }
        _StreamTextPhone.Viewers.text = viewers.ToString();
        if (viewers > 0)
        {
            float moneyPlus = viewers * moneyPlusPerViwer;
            UIController.instance.PlusMoney(moneyPlus);
        }
    }
    private void FixedUpdate()
    {
        if (isActiveStreamScene)
        {
            if (isStreamOn)
            {
                timerSteam += 1f * Time.fixedDeltaTime;
                _StreamTextPhone.upTime.text =TimeFormatter(timerSteam,true);
                if (lastCheckTime + 1f < timerSteam)
                {
                    lastCheckTime = timerSteam;
                    CalculateMoney();
                }
                if (timer < timePerChat)
                {
                    timer += 1f * Time.fixedDeltaTime;
                }
                else
                {
                    timePerChat = Random.Range(minTime, maxTime);
                    timer = 0;
                    if (viewers > 0)
                    {
                        SpawnChatItem();
                    }
                }
            }
        }
    }
    
    public string TimeFormatter( float seconds, bool forceHHMMSS = false)
    {
        float secondsRemainder = Mathf.Floor( (seconds % 60) * 100) / 100.0f;
        int minutes = ((int)(seconds / 60)) % 60;
        int hours = (int)(seconds / 3600);
 
        if (!forceHHMMSS)
        {
            if (hours == 0)
            {
                return System.String.Format ("{0:00}:{1:00.00}", minutes, secondsRemainder);
            }
        }
        return System.String.Format ("{0:00}:{1:00}:{2:00}", hours, minutes, secondsRemainder);
    }

    public void SpawnChatItem()
    {
        if (_currentPhone != null)
        {
            Sprite icon = null;
            int colorID = Random.Range(0, nameColors.Count);
            if (streamState == StreamState.idle)
            {
                icon = idleIcons[Random.Range(0, idleIcons.Count)];
            }
            if (streamState == StreamState.monster)
            {
                icon = monsterIcons[Random.Range(0, monsterIcons.Count)];
            }
            if (streamState == StreamState.monsterIsStoped)
            {
                icon = monsterIsStopedIcons[Random.Range(0, monsterIsStopedIcons.Count)];
            }
            _currentPhone.SpawnChatItem(nameColors[colorID],icon);
        }
    }
}
