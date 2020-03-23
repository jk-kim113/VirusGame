﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IPointerClickHandler, IBeginDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        eventData.pointerEnter = gameObject;

        
        transform.position = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.pointerEnter = gameObject;
        transform.position = eventData.position;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            transform.position = Input.mousePosition;
        }
    }
}
