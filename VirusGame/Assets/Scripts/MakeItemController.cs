using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeItemController : MonoBehaviour
{
    public static MakeItemController Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mMakerUseItem;
    [SerializeField]
    private Transform mUseItemScrollTarget;
    [SerializeField]
    private GameObject mAskFoodType;
    [SerializeField]
    private GameObject mConfirmFoodType;

    [SerializeField]
    private GameObject mMakerEquipItem;
    [SerializeField]
    private Transform mEquipItemScrollTarget;

    [SerializeField]
    private CombinationElement mCombElement;
#pragma warning restore

    private List<CombinationElement> mUseItemEleList = new List<CombinationElement>();
    List<CombinationElement> mEquipItemEleList = new List<CombinationElement>();

    private int mNewItemID;

    private UseItemData[] mUseItemDataArr;
    private EquipItemData[] mEquipItemDataArr;
    
    private Sprite[] mUseItemSpriteArr;
    private Sprite[] mEquipItemSpriteArr;

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
        JsonDataLoader.Instance.LoadJsonData<UseItemData>(out mUseItemDataArr, "JsonFiles/UseItemData");
        JsonDataLoader.Instance.LoadJsonData<EquipItemData>(out mEquipItemDataArr, "JsonFiles/EquipItemData");
        JsonDataLoader.Instance.LoadJsonData<ItemMakingInfo>(out mItemMakingInfoArr, "JsonFiles/ItemMakingInfoData");
        mUseItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/UseItem");
        mEquipItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/EquipItem");

        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        while (!DataGroup.Instance.Loaded)
        {
            yield return term;
        }

        InitUseItemElement();
        InitEquipItemElement();
    }

    private void InitUseItemElement()
    {
        for (int i = 0; i < mUseItemDataArr.Length; i++)
        {
            CombinationElement element = Instantiate(mCombElement, mUseItemScrollTarget);
            element.Init(
                mUseItemSpriteArr[i],
                mUseItemDataArr[i].ID,
                mUseItemDataArr[i].Name,
                mUseItemDataArr[i].Info,
                mItemMakingInfoArr[i].NeedNumber,
                mItemMakingInfoArr[i].NeedID,
                MakeUseItem);
            mUseItemEleList.Add(element);
        }
    }

    private void InitEquipItemElement()
    {
        for (int i = 0; i < mEquipItemDataArr.Length; i++)
        {
            CombinationElement element = Instantiate(mCombElement, mEquipItemScrollTarget);
            element.Init(
                mEquipItemSpriteArr[i],
                mEquipItemDataArr[i].ID,
                mEquipItemDataArr[i].Name,
                mEquipItemDataArr[i].Info,
                mItemMakingInfoArr[i + 2].NeedNumber,
                mItemMakingInfoArr[i + 2].NeedID,
                MakeEquipItem);
            mEquipItemEleList.Add(element);
        }
    }

    #region  UseItem Part
    private void MakeUseItem(int newItemID)
    {
        if (InvenController.Instance.CheckIsFull(newItemID))
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
            mUseItemDataArr[mNewItemID - 3000].TypeValue[type],
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

        RenewUseItemElement();

        mConfirmFoodType.SetActive(false);
    }

    public void OpenUseItemMaker(bool value)
    {
        mMakerUseItem.SetActive(value);

        RenewUseItemElement();
    }

    private void RenewUseItemElement()
    {
        for (int i = 0; i < mUseItemEleList.Count; i++)
        {
            mUseItemEleList[i].Renew(
                mItemMakingInfoArr[i].NeedNumber,
                mItemMakingInfoArr[i].NeedID
                );
        }
    }
    #endregion

    #region EquipItem Part

    private void MakeEquipItem(int newItemID)
    {
        if (InvenController.Instance.CheckIsFull(newItemID))
        {
            // Do not make Item
        }
        else
        {
            mNewItemID = newItemID;

            for (int i = 0; i < mItemMakingInfoArr[newItemID - 4000 + 2].NeedNumber.Length; i++)
            {
                int needID = mItemMakingInfoArr[newItemID - 4000 + 2].NeedID[i];
                int needNum = mItemMakingInfoArr[newItemID - 4000 + 2].NeedNumber[i];

                if (needNum > 0)
                {
                    DataGroup.Instance.SetItemNumber(needID, -needNum);

                    InvenController.Instance.RenewInven(needID);
                }
            }

            RenewEquipItemElement();

            InvenController.Instance.SetSpriteToInven(newItemID, 1);
        }
    }

    private void RenewEquipItemElement()
    {
        for (int i = 0; i < mEquipItemEleList.Count; i++)
        {
            mEquipItemEleList[i].Renew(
                mItemMakingInfoArr[mNewItemID - 4000 + 2].NeedNumber,
                mItemMakingInfoArr[mNewItemID - 4000 + 2].NeedID
                );
        }
    }

    public void OpenEquipItemMaker(bool value)
    {
        mMakerEquipItem.SetActive(value);

        RenewEquipItemElement();
    }

    #endregion

}
