﻿using System.Collections;
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
    [SerializeField]
    private GameObject mAskFoodType;
    [SerializeField]
    private GameObject mConfirmFoodType;
#pragma warning restore

    private List<CombinationElement> mCombEleList;
    private int mNewItemID;
    private eFoodType mFoodType;

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
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        while(!DataGroup.Instance.Loaded)
        {
            yield return term;
        }

        for (int i = 0; i < DataGroup.Instance.ItemDataDic["Comb"].Length; i++)
        {
            CombinationElement element = Instantiate(mCombElement, mScrollTarget);
            element.Init(
                DataGroup.Instance.ItemSpriteDic[DataGroup.Instance.ItemDataDic["Comb"][i].ID],
                DataGroup.Instance.ItemDataDic["Comb"][i].ID,
                DataGroup.Instance.ItemDataDic["Comb"][i].Name,
                DataGroup.Instance.ItemDataDic["Comb"][i].Info,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.ItemDataDic["Comb"][i].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.ItemDataDic["Comb"][i].ID].NeedID,
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
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.ItemDataDic["Comb"][i].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.ItemDataDic["Comb"][i].ID].NeedID
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
        mAskFoodType.SetActive(false);
        mConfirmFoodType.SetActive(true);
        mFoodType = (eFoodType)type;
    }

    public void ConfirmMakeFood()
    {
        for (int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber.Length; i++)
        {
            if (DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber[i] > 0)
            {
                DataGroup.Instance.SetItemNumber(
                    DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedID[i],
                    -DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber[i]);
            }
        }

        InvenController.Instance.RenewInven(mNewItemID);
        InvenController.Instance.SetSpriteToInven(mNewItemID, 1);

        RenewCombTable();

        switch (mFoodType)
        {
            case eFoodType.Raw:
                //DataGroup.Instance.FoodMakeTypeDic[mNewItemID].RawValue
                break;
            case eFoodType.Fried:
                break;
            case eFoodType.Steamed:
                break;
            default:
                break;
        }
    }
}

public enum eFoodType
{
    Raw,
    Fried,
    Steamed
}
