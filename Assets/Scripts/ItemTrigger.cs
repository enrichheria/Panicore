using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemTrigger : MonoBehaviour
{
    public int id;
    public UnityEvent actionEnter;
    public GameObject builder;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            if (builder.activeSelf == false)
            {
                other.GetComponentInParent<AI_Controller>().SetMonsterEntered(id, transform.parent.gameObject);
                if (actionEnter != null)
                {
                    actionEnter.Invoke();
                }
            }
        }
    }
}
