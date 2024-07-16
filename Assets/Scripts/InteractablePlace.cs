using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class InteractablePlace : MonoBehaviour
{
    public Material buildMaterial;
    public Transform pivotCast;
    public Vector3 castSize;
    public Color gizmoColor;
    public bool isActive;
    public bool isCanPlace;
    private bool lastState;
    public LayerMask _LayerMask;
    private bool isRotate;

    public Color green, red;
    
    public bool isRayCast;

    public List<Collider> _hits = new List<Collider>();
    private void Start()
    {
        lastState = true;
    }

    private void OnEnable()
    {
        isActive = true;
    }
    private void OnDisable()
    {
        isActive = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.matrix = pivotCast.localToWorldMatrix; 
        Gizmos.DrawCube(Vector3.zero, castSize);
    }

    public void Rotate()
    {
        if (!isRotate)
        {
            isRotate = true;
            Vector3 localAxis = transform.localEulerAngles;
            localAxis.y += 45;
            transform.DOLocalRotate(localAxis, 0.5f).OnComplete(() =>
            {
                isRotate = false;
            });
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            if (isRayCast)
            {
                isCanPlace = Physics.OverlapBox(pivotCast.position, castSize / 2f, pivotCast.rotation, _LayerMask)
                    .Length == 0;
                if (isCanPlace)
                {
                    buildMaterial.color =green;
                }
                else
                {
                    buildMaterial.color = red;
                }
            }
            else
            {
                isCanPlace = false;
                buildMaterial.color = red;
            }
        }
    }

    void CheckState()
    {
        if (lastState != isCanPlace)
        {
            lastState = isCanPlace;
            
        }
    }
}
