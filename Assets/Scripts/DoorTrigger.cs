using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door _door;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            _door.OpenCloseDoorMonster();
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            _door.OpenCloseDoorMonster();
        }
    }
}
