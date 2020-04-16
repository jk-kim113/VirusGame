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
    private int mSelcetAreaPosNum;

    private void Start()
    {
        mSlotArr = GetComponentsInChildren<Slot>();

        mSelcetAreaPosNum = 0;
        mSelectAreaPos.transform.position = mSlotArr[mSelcetAreaPosNum].gameObject.transform.position;
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

    public void RenewInven(int originalID)
    {
        for(int i = 0; i < mSlotArr.Length; i++)
        {
            if(mSlotArr[i].ItemID == originalID)
            {
                mSlotArr[i].Renew(DataGroup.Instance.ItemNumDic[originalID]);
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
            mSelcetAreaPosNum++;
            if(mSelcetAreaPosNum >= mSlotArr.Length)
            {
                mSelcetAreaPosNum = 0;
            }
            mSelectAreaPos.transform.position = mSlotArr[mSelcetAreaPosNum].gameObject.transform.position;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            if(mSlotArr[mSelcetAreaPosNum].ItemID < 0)
            {
                return;
            }

            DataGroup.Instance.SetItemNumber(mSlotArr[mSelcetAreaPosNum].ItemID, -1);
            int virusID = InvenController.Instance.CheckIsInfected(mSlotArr[mSelcetAreaPosNum].ItemID);

            eItemType type = DataGroup.Instance.ItemTypeDic[mSlotArr[mSelcetAreaPosNum].ItemID];

            switch(type)
            {
                case eItemType.Drop:
                    
                    break;
                case eItemType.Use:

                    eUseTarget target = DataGroup.Instance.FoodMenuDic[IDtoUseType(mSlotArr[mSelcetAreaPosNum].ItemID)].UseTarget;

                    Player.Instance.UseItem(
                        target,
                        DataGroup.Instance.FoodMakeTypeDic[IDtoUseType(mSlotArr[mSelcetAreaPosNum].ItemID)].TypeValue[(int)target],
                        virusID);

                    break;
                case eItemType.Equip:
                    break;
                default:
                    break;
            }
        }
    }

    private int IDtoUseType(int targetid)
    {
        string targetStr = targetid.ToString();
        char[] targetCharArr = targetStr.ToCharArray();

        string Index = targetCharArr[0].ToString() + targetCharArr[1].ToString() + targetCharArr[2].ToString() + targetCharArr[3].ToString();

        return int.Parse(Index);
    }
}
