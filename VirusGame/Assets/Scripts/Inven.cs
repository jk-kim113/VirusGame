using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject mSelectAreaPos;
#pragma warning restore

    private Slot[] mSlotArr;
    private int mSelectAreaPosNum;

    private void Start()
    {
        mSlotArr = GetComponentsInChildren<Slot>();

        mSelectAreaPosNum = 0;
        mSelectAreaPos.transform.position = mSlotArr[mSelectAreaPosNum].gameObject.transform.position;
    }

    public void GetItem(int originalId, int num, eItemType itemType)
    {
        Sprite img = DataGroup.Instance.SpriteDataDic[itemType][originalId];

        switch (itemType)
        {
            case eItemType.Drop:
            case eItemType.Use:
                for (int i = 0; i < mSlotArr.Length; i++)
                {
                    if (mSlotArr[i].ItemID == originalId)
                    {
                        mSlotArr[i].Init(img, num, originalId, itemType);
                        return;
                    }
                }

                for (int i = 0; i < mSlotArr.Length; i++)
                {
                    if (!mSlotArr[i].IsFull)
                    {
                        mSlotArr[i].Init(img, num, originalId, itemType);
                        return;
                    }
                }
                break;
            case eItemType.Equip:
                for (int i = 0; i < mSlotArr.Length; i++)
                {
                    if (!mSlotArr[i].IsFull)
                    {
                        mSlotArr[i].Init(img, num, originalId, itemType);
                        return;
                    }
                }
                break;
            default:
                Debug.LogError("Wrong item type : " + itemType);
                break;
        }
    }

    public void RenewInven(int originalID, eItemType itemType)
    {
        for(int i = 0; i < mSlotArr.Length; i++)
        {
            if(mSlotArr[i].ItemID == originalID && mSlotArr[i].ItemType == itemType)
            {   
                mSlotArr[i].Renew(InvenController.Instance.ItemNumberDic[itemType][originalID]);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            mSelectAreaPosNum++;
            if(mSelectAreaPosNum >= mSlotArr.Length)
            {
                mSelectAreaPosNum = 0;
            }
            mSelectAreaPos.transform.position = mSlotArr[mSelectAreaPosNum].gameObject.transform.position;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if (mSlotArr[mSelectAreaPosNum].ItemID < 0)
            {
                return;
            }

            if (mSlotArr[mSelectAreaPosNum].ItemType == eItemType.Use)
            {
                InvenController.Instance.SettingItemNumber(eItemType.Use, mSlotArr[mSelectAreaPosNum].ItemID, -1);
                eUseTarget target = DataGroup.Instance.UseItemDataDic[mSlotArr[mSelectAreaPosNum].ItemID].UseTarget;
                Player.Instance.UseItem(
                    target,
                    DataGroup.Instance.UseItemDataDic[mSlotArr[mSelectAreaPosNum].ItemID].TypeValue[(int)target]);
            }
        }
    }
}
