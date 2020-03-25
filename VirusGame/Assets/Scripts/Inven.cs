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

    public void GetItem(Sprite sprite, int num, int originalId, string tag)
    {
        for(int i = 0; i < mSlotPoint; i++)
        {
            if(mSlotArr[i].ItemID == originalId)
            {
                mSlotArr[i].Init(sprite, num, originalId, tag);
                return;
            }
        }
        
        mSlotArr[mSlotPoint].Init(sprite, num, originalId, tag);
        mSlotPoint++;
    }

    public bool CheckIsFull(int originalID)
    {
        if(mSlotPoint <= mSlotArr.Length)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < mSlotArr.Length; i++)
            {
                if(mSlotArr[i].ItemID == originalID)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
