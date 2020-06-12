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
    private List<CombinationElement> mEquipItemEleList = new List<CombinationElement>();

    private int mNewItemID;

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
        foreach(UseItemData data in DataGroup.Instance.UseItemDataDic.Values)
        {
            CombinationElement element = Instantiate(mCombElement, mUseItemScrollTarget);
            element.Init(
                DataGroup.Instance.SpriteDataDic[eItemType.Use][data.ID],
                data.ID,
                data.Name,
                data.Info,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][data.ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][data.ID].NeedID,
                MakeUseItem);
            mUseItemEleList.Add(element);
        }
    }

    private void InitEquipItemElement()
    {
        foreach(EquipItemData data in DataGroup.Instance.EquipItemDataDic.Values)
        {
            CombinationElement element = Instantiate(mCombElement, mEquipItemScrollTarget);
            element.Init(
                DataGroup.Instance.SpriteDataDic[eItemType.Equip][data.ID],
                data.ID,
                data.Name,
                data.Info,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][data.ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][data.ID].NeedID,
                MakeEquipItem);
            mEquipItemEleList.Add(element);
        }
    }

    #region  "UseItem Part"
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
            DataGroup.Instance.UseItemDataDic[mNewItemID].TypeValue[type],
            0);
    }

    public void ConfirmMakeFood()
    {
        for (int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][mNewItemID].NeedNumber.Length; i++)
        {
            int needID = DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][mNewItemID].NeedID[i];
            int needNum = DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][mNewItemID].NeedNumber[i];

            if (needNum > 0)
            {
                if(InvenController.Instance.ItemNumberDic[eItemType.Use].ContainsKey(needID))
                {
                    if (needNum > InvenController.Instance.ItemNumberDic[eItemType.Use][needID])
                    {
                        Debug.Log("No Need Item");
                        return;
                    }

                    InvenController.Instance.SettingItemNumber(eItemType.Use, mNewItemID, -needNum);
                    InvenController.Instance.RenewInven(needNum); //TODO 
                }
                else
                {
                    Debug.Log("No Need Item");
                    return;
                }
                
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
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][mUseItemEleList[i].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Use][mUseItemEleList[i].ID].NeedID
                );
        }
    }
    #endregion

    #region "EquipItem Part"

    private void MakeEquipItem(int newItemID)
    {
        if (InvenController.Instance.CheckIsFull(newItemID))
        {
            // Do not make Item
        }
        else
        {
            mNewItemID = newItemID;

            for (int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][mNewItemID].NeedNumber.Length; i++)
            {
                int needID = DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][mNewItemID].NeedID[i];
                int needNum = DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][mNewItemID].NeedNumber[i];

                if (needNum > 0)
                {
                    if(InvenController.Instance.ItemNumberDic[eItemType.Use].ContainsKey(needID))
                    {
                        if (needNum > InvenController.Instance.ItemNumberDic[eItemType.Use][needID])
                        {
                            Debug.Log("No Need Item");
                            return;
                        }

                        InvenController.Instance.SettingItemNumber(eItemType.Equip, mNewItemID, -needNum);
                        InvenController.Instance.RenewInven(needID); //TODO
                    }
                    else
                    {
                        Debug.Log("No Need Item");
                        return;
                    }
                }
            }

            RenewEquipItemElement();

            //InvenController.Instance.SetSpriteToInven(mNewItemID, 1);
        }
    }

    private void RenewEquipItemElement()
    {
        for (int i = 0; i < mEquipItemEleList.Count; i++)
        {
            mEquipItemEleList[i].Renew(
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][mEquipItemEleList[i].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[eItemType.Equip][mEquipItemEleList[i].ID].NeedID
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
