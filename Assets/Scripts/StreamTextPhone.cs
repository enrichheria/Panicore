using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StreamTextPhone : MonoBehaviour
{
    public TMP_Text upTime, Viewers;

    private void Start()
    {
        StreamController.instance._StreamTextPhone = this;
    }
}
