using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoCar : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LevelLoader.instance.LoadLevel(2);
        }
    }
}
