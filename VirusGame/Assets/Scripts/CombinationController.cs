using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationController : MonoBehaviour
{
    public static CombinationController Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mItemCombTable;
    [SerializeField]
    private Transform mScrollTarget;
    [SerializeField]
    private CombinationElement mCombElement;
#pragma warning restore

    private CombItem[] mCombItemArr;
    private ItemMakingInfo[] mItemMakingInfoArr;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mCombItemArr = new CombItem[1];

        mCombItemArr[0] = new CombItem();
        mCombItemArr[0].Name = "샐러드";
        mCombItemArr[0].ID = 3000;
        mCombItemArr[0].Content = "섭취 시 배고픔을 20 회복 시켜준다.";

        mItemMakingInfoArr = new ItemMakingInfo[1];

        mItemMakingInfoArr[0] = new ItemMakingInfo();
        mItemMakingInfoArr[0].TargetID = 3000;
        mItemMakingInfoArr[0].NeedID = new int[4];
        mItemMakingInfoArr[0].NeedID[0] = new int();
        mItemMakingInfoArr[0].NeedID[0] = 1000;
    }

    public void OpenCombTable(bool value)
    {
        mItemCombTable.SetActive(value);

        if(value)
        {
            for(int i = 0; i < mCombItemArr.Length; i++)
            {
                int targetID = -1;
                for(int j = 0; j < mItemMakingInfoArr.Length; j++)
                {
                    if(mCombItemArr[i].ID == mItemMakingInfoArr[j].TargetID)
                    {
                        targetID = j;
                        break;
                    }
                }

                CombinationElement element = Instantiate(mCombElement, mScrollTarget);
                element.Init();
            }
        }
    }
}

public class CombItem
{
    public string Name;
    public int ID;
    public string Content;
}

public class ItemMakingInfo
{
    public int TargetID;
    public int[] NeedID;
    public int[] NeedNumber;
}