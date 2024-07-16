using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSender : MonoBehaviour
{
    public static EventSender instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SendEventStartScene(string value)
    {
        Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
        _eventParameters.Add("scene_name",value);
        AppMetrica.Instance.ReportEvent("level_start",_eventParameters);
        AppMetrica.Instance.SendEventsBuffer();
    }
    
    public void SendEventExitGame(bool isDead, int lifeTime)
    {
        Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
        if (isDead)
        {
            _eventParameters.Add("result", "fail");
        }
        else
        {
            _eventParameters.Add("result", "complete");
        }
        _eventParameters.Add("time_spent", lifeTime);
        AppMetrica.Instance.ReportEvent("level_finish",_eventParameters);
        AppMetrica.Instance.SendEventsBuffer();
    }
    
    public void SendEventBuyItem(string value)
    {
        Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
        _eventParameters.Add("item_name",value);
        AppMetrica.Instance.ReportEvent("buy_item",_eventParameters);
    }
    
    public void SendEventCollectItem(string value)
    {
        Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
        _eventParameters.Add("item_name",value);
        AppMetrica.Instance.ReportEvent("collect_Item",_eventParameters);
    }
    
    public void SendEventCraftItem(string value)
    {
        Dictionary<string, object> _eventParameters = new Dictionary<string, object>();
        _eventParameters.Add("item_name",value);
        AppMetrica.Instance.ReportEvent("craft_Item",_eventParameters);
    }
    
    public void SendEventStreamOn()
    {
        AppMetrica.Instance.ReportEvent("stream_on");
    }
    
}
