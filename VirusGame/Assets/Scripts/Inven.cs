using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    private Slot[] mSlotArr;

    private void Start()
    {
        mSlotArr = GetComponentsInChildren<Slot>();
    }

    public void GetItem(Sprite sprite, int num, int originalId)
    {
        for(int i = 0; i < mSlotArr.Length; i++)
        {
            if (mSlotArr[i].ItemID == originalId)
            {
                mSlotArr[i].Init(sprite, num, originalId);
                return;
            }
        }

        for (int i = 0; i < mSlotArr.Length; i++)
        {
            if (!mSlotArr[i].IsFull)
            {
                mSlotArr[i].Init(sprite, num, originalId);
                return;
            }
        }
    }

    public bool CheckIsFull(int originalID)
    {
        for (int i = 0; i < mSlotArr.Length; i++)
        {
            if (mSlotArr[i].ItemID == originalID)
            {
                return false;
            }
        }

        for (int i = 0; i < mSlotArr.Length; i++)
        {
            if (!mSlotArr[i].IsFull)
            {
                return false;
            }
        }

        return true;
    }
}
