using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Outline _Outline;
    private bool isActiveOutLine;

    public Rigidbody _Rigidbody;
    
    public ItemType Type;
    public int ID = 0;

    public GameObject PlaceObgect, Mesh;
    
    void Start()
    {
        
    }

    public void ActivePlaceObject(bool value)
    {
        GetComponent<Collider>().enabled = !value;
        PlaceObgect.SetActive(value);
        Mesh.SetActive(!value);
    }
    
    public void ActiveOutLine(bool value)
    {
        if (value != isActiveOutLine)
        {
            isActiveOutLine = value;
            _Outline.enabled = isActiveOutLine;
            if (isActiveOutLine)
            {
                gameObject.layer = LayerMask.NameToLayer("ItemsOutlined");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Items");
            }
        }
    }

    public void Collect()
    {
        if (_Rigidbody != null)
        {
            _Rigidbody.isKinematic = false;
        }
    }

    public void Drop(Vector3 direction, bool isDropInBag)
    {
        if (Type == ItemType.part)
        {
            _Rigidbody.AddForce(direction * 3f, ForceMode.Impulse);
        }
        else
        {
            if (!isDropInBag)
            {
                PlayerController.instance.SetInteractivePlace(transform);
            }
            else
            {
                transform.position = PlayerController.instance.transform.position;
            }
        }
    }
}
