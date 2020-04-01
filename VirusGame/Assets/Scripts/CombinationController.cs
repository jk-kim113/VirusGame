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

    private List<CombinationElement> mCombEleList;

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
            for(int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber.Length; i++)
            {
                if (DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber[i] > 0)
                {
                    DataGroup.Instance.SetItemNumber(
                        DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedID[i], 
                        -DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber[i]);
                }
            }

            InvenController.Instance.SetSpriteToInven(newItemID, 1);

            RenewCombTable();
        }
    }
}