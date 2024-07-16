using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    public Cell myCell;

    public Canvas canvas;
    private RectTransform _rectTransform;
    public float distanceToDrop;
    private Vector2 fixedPos;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        fixedPos = _rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log(Vector2.Distance(fixedPos, _rectTransform.anchoredPosition));
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(fixedPos, _rectTransform.anchoredPosition) > distanceToDrop)
        {
            myCell.DropItem();
        }
        _rectTransform.anchoredPosition = fixedPos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
