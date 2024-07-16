using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    void Start()
    {
        if (AI_Controller.instance != null)
        {
            AI_Controller.instance.SetRadio(transform);
        }
    }

    
}
