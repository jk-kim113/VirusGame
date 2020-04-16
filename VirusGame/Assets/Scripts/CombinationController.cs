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

        for (int i = 0; i < DataGroup.Instance.FoodMenuDic.Count; i++)
        {
            int id = DataGroup.Instance.FoodMenuArr[i].ID;

            CombinationElement element = Instantiate(mCombElement, mScrollTarget);
            element.Init(
                DataGroup.Instance.ItemSpriteDic[id],
                DataGroup.Instance.FoodMenuDic[id].ID,
                DataGroup.Instance.FoodMenuDic[id].Name,
                DataGroup.Instance.FoodMenuDic[id].Info,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.FoodMenuDic[id].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.FoodMenuDic[id].ID].NeedID,
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
            int id = DataGroup.Instance.FoodMenuArr[i].ID;

            mCombEleList[i].Renew(
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.FoodMenuDic[id].ID].NeedNumber,
                DataGroup.Instance.ItemMakingInfoDic[DataGroup.Instance.FoodMenuDic[id].ID].NeedID
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
            OnOffAskFoodType(true);
        }
    }

    public void SelectFoodType(int type)
    {
        OnOffAskFoodType(false);
        OnOffConfirmFoodType(true);
        mFoodType = (eFoodType)type;

        Text message = mConfirmFoodType.GetComponentInChildren<Text>();
        message.text = string.Format("해당 제조법의 음식 효과는 {0} 이고, 소비 전력량은 {1} 입니다.", 
            DataGroup.Instance.FoodMakeTypeDic[mNewItemID].TypeValue[type],
            0);
    }

    public void ConfirmMakeFood()
    {
        bool isInfect = false;
        int virusID = -999;

        for (int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber.Length; i++)
        {
            int needID = DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedID[i];
            int needNum = DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber[i];

            if (DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedNumber[i] > 0)
            {
                DataGroup.Instance.SetItemNumber(
                    needID,
                    -needNum);

                InvenController.Instance.RenewInven(DataGroup.Instance.ItemMakingInfoDic[mNewItemID].NeedID[i]);
            }

            int temp = InvenController.Instance.InvenVirusInfoDic[needID].Count;

            for (int j = temp; j > temp - needNum; j--)
            {
                if(!isInfect)
                {
                    if (InvenController.Instance.InvenVirusInfoDic[needID][temp - 1] > 0)
                    {
                        isInfect = true;
                        virusID = InvenController.Instance.InvenVirusInfoDic[needID][temp - 1];
                    }
                }
            }

            InvenController.Instance.RemoveInvenVirusInfo(needID, needNum);
        }

        mNewItemID = DataGroup.Instance.FoodMenuDic[mNewItemID].TargetID[(int)mFoodType];

        RenewCombTable();

        InvenController.Instance.SetSpriteToInven(mNewItemID, 1, virusID);

        

        mConfirmFoodType.SetActive(false);
    }

    public void OnOffAskFoodType(bool value)
    {
        mAskFoodType.SetActive(value);
    }

    public void OnOffConfirmFoodType(bool value)
    {
        mConfirmFoodType.SetActive(value);
    }
}
