using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject mAskFoodType;
    [SerializeField]
    private GameObject mConfirmFoodType;
#pragma warning restore

    private List<CombinationElement> mCombEleList;
    private int mNewItemID;

    private UseItemData[] mUseItemDataAtrr;
    private ItemMakingInfo[] mItemMakingInfoArr;
    private Sprite[] mUseItemSpriteArr;

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

        mCombEleList = new List<CombinationElement>();
    }

    private void Start()
    {
        JsonDataLoader.Instance.LoadJsonData<UseItemData>(out mUseItemDataAtrr, "JsonFiles/UseItemData");
        JsonDataLoader.Instance.LoadJsonData<ItemMakingInfo>(out mItemMakingInfoArr, "JsonFiles/ItemMakingInfoData");
        mUseItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/UseItem");

        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        while(!DataGroup.Instance.Loaded)
        {
            yield return term;
        }

        for (int i = 0; i < mUseItemDataAtrr.Length; i++)
        {
            CombinationElement element = Instantiate(mCombElement, mScrollTarget);
            element.Init(
                mUseItemSpriteArr[i],
                mUseItemDataAtrr[i].ID,
                mUseItemDataAtrr[i].Name,
                mUseItemDataAtrr[i].Info,
                mItemMakingInfoArr[i].NeedNumber,
                mItemMakingInfoArr[i].NeedID,
                MakeItem);
            mCombEleList.Add(element);
        }
    }
    
    public void OpenCombTable(bool value)
    {
        mItemCombTable.SetActive(value);

        RenewCombTable();
    }

    private void RenewCombTable()
    {
        for (int i = 0; i < mCombEleList.Count; i++)
        {
            mCombEleList[i].Renew(
                mItemMakingInfoArr[i].NeedNumber,
                mItemMakingInfoArr[i].NeedID
                );
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
            mNewItemID = newItemID;
            mAskFoodType.SetActive(true);
        }
    }

    public void SelectFoodType(int type)
    {
        mConfirmFoodType.SetActive(true);

        Text message = mConfirmFoodType.GetComponentInChildren<Text>();
        message.text = string.Format("해당 제조법의 음식 효과는 {0} 이고, 소비 전력량은 {1} 입니다.",
            mUseItemDataAtrr[mNewItemID - 3000].TypeValue[type],
            0);
    }

    public void ConfirmMakeFood()
    {
        for (int i = 0; i < mItemMakingInfoArr[mNewItemID - 3000].NeedNumber.Length; i++)
        {
            int needID = mItemMakingInfoArr[mNewItemID - 3000].NeedID[i];
            int needNum = mItemMakingInfoArr[mNewItemID - 3000].NeedNumber[i];

            if (needNum > 0)
            {
                if (needNum > DataGroup.Instance.ItemNumDic[needID])
                {
                    Debug.Log("No Need Item");
                    return;
                }

                DataGroup.Instance.SetItemNumber(
                    needID,
                    -needNum);

                InvenController.Instance.RenewInven(needNum);
            }
        }

        RenewCombTable();

        mConfirmFoodType.SetActive(false);
    }

}
