using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMaker : MonoBehaviour
{
    public static EquipMaker Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mEquipMakerUI;
    [SerializeField]
    private Transform mScrollTarget;
    [SerializeField]
    private CombinationElement mElement;
#pragma warning restore

    List<CombinationElement> mElementList = new List<CombinationElement>();

    private Equip[] mEquipArr;
    int[] needNum = new int[4];
    int[] needID = new int[4];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mEquipArr = new Equip[1];

        mEquipArr[0] = new Equip();
        mEquipArr[0].Name = "비커";
        mEquipArr[0].ID = 5000;
        mEquipArr[0].Info = "혈액 채취를 하기 위해 필요한 도구";
    }

    private void Start()
    {   
        needNum[0] = 3;
        needNum[1] = 0;
        needNum[2] = 0;
        needNum[3] = 0;
        
        needID[0] = 1001;
        needID[1] = 0;
        needID[2] = 0;
        needID[3] = 0;

        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        while (!DataGroup.Instance.Loaded)
        {
            yield return term;
        }

        for (int i = 0; i < mEquipArr.Length; i++)
        {
            CombinationElement element = Instantiate(mElement, mScrollTarget);
            element.Init(null, mEquipArr[i].ID, mEquipArr[i].Name, mEquipArr[i].Info, needNum, needID, MakeItem);
            mElementList.Add(element);
        }
    }

    private void MakeItem(int newItemID)
    {
        if (InvenController.Instance.CheckIsFull(newItemID))
        {
            // Do not make Item
        }
        else
        {
            for (int i = 0; i < DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber.Length; i++)
            {
                int needID = DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedID[i];
                int needNum = DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber[i];

                if (DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedNumber[i] > 0)
                {
                    DataGroup.Instance.SetItemNumber(
                        needID,
                        -needNum);

                    InvenController.Instance.RenewInven(DataGroup.Instance.ItemMakingInfoDic[newItemID].NeedID[i]);
                }

                int temp = InvenController.Instance.InvenVirusInfoDic[needID].Count;

                InvenController.Instance.RemoveInvenVirusInfo(needID, needNum);
            }

            RenewCombTable();

            InvenController.Instance.SetSpriteToInven(newItemID, 1, 0);
        }
    }

    private void RenewCombTable()
    {
        for (int i = 0; i < mElementList.Count; i++)
        {
            mElementList[i].Renew(
                needNum,
                needID
                );
        }
    }

    public void OpenEquipMaker(bool value)
    {
        mEquipMakerUI.SetActive(value);
    }

}

public class Equip
{
    public string Name;
    public int ID;
    public string Info;
}