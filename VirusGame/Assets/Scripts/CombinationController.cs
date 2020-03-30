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
    }

    private void Start()
    {
        JsonDataLoader.Instance.LoadJsonData<CombItem>(out mCombItemArr, "JsonFiles/CombItemData");
        JsonDataLoader.Instance.LoadJsonData<ItemMakingInfo>(out mItemMakingInfoArr, "JsonFiles/CombinationData");
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
                element.Init(
                    null,
                    mCombItemArr[i].Name,
                    mCombItemArr[i].Content,
                    mItemMakingInfoArr[targetID].NeedNumber,
                    mItemMakingInfoArr[targetID].NeedID);
            }
        }
    }

    private void MakeItem(int newItemID)
    {
        if(InvenController.Instance.CheckIsFull(newItemID))
        {
            // Do not make Item
        }
        else
        {
            for(int i = 0; i < mItemMakingInfoArr.Length; i++)
            {
                if(mItemMakingInfoArr[i].TargetID == newItemID)
                {
                    for(int j = 0; j < mItemMakingInfoArr[i].NeedID.Length; j++)
                    {
                        if(mItemMakingInfoArr[i].NeedID[j] > 0)
                        {
                            DataGroup.Instance.SetItemNumber(mItemMakingInfoArr[i].NeedID[j], -mItemMakingInfoArr[i].NeedNumber[j]);
                        }
                    }
                }
            }

            //InvenController.Instance.SetSpriteToInven()
        }
    }
}