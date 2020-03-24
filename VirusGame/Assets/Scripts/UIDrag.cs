using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour
{
    private Image mImg;

    private void Awake()
    {
        mImg = GetComponent<Image>();
        mImg.enabled = false;
    }

    public void OnImage(Sprite value)
    {
        mImg.enabled = true;
        mImg.sprite = value;
    }

    public void OffImage()
    {
        mImg.enabled = false;
    }
}
