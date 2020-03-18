using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mItemImg;
    [SerializeField]
    private Text mItemNum;
#pragma warning restore

    private int mItemID;
    public int ItemID { get { return mItemID; } }

    private void Awake()
    {
        mItemImg.enabled = false;
        mItemNum.enabled = false;
        mItemID = -999;
    }

    public void Init(Sprite img, int num, int id)
    {
        mItemImg.enabled = true;
        mItemNum.enabled = true;

        mItemID = id;
        mItemImg.sprite = img;
        mItemNum.text = num.ToString();
    }
}
