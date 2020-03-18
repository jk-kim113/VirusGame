using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    private Slot[] mSlotArr;
    private int mSlotPoint;

    private void Awake()
    {
        mSlotPoint = 0;
    }

    private void Start()
    {
        mSlotArr = GetComponentsInChildren<Slot>();
    }

    public void GetItem(Sprite sprite, int num, int id)
    {
        for(int i = 0; i < mSlotPoint; i++)
        {
            if(mSlotArr[i].ItemID == id)
            {
                mSlotArr[i].Init(sprite, num, id);
                return;
            }
        }

        mSlotArr[mSlotPoint].Init(sprite, num, id);
    }
}
