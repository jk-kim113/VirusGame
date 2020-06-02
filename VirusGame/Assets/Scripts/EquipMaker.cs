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
    int[] needNumArr = new int[4];
    int[] needIDArr = new int[4];

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
        needNumArr[0] = 3;
        needNumArr[1] = 0;
        needNumArr[2] = 0;
        needNumArr[3] = 0;

        needIDArr[0] = 1001;
        needIDArr[1] = 0;
        needIDArr[2] = 0;
        needIDArr[3] = 0;

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
            element.Init(null, mEquipArr[i].ID, mEquipArr[i].Name, mEquipArr[i].Info, needNumArr, needIDArr, MakeItem);
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
            for (int i = 0; i < needNumArr.Length; i++)
            {   
                int needID = needIDArr[i];
                int needNum = needNumArr[i];

                if (needNumArr[i] > 0)
                {
                    DataGroup.Instance.SetItemNumber(needID,-needNum);

                    InvenController.Instance.RenewInven(needIDArr[i]);
                }
            }

            RenewCombTable();

            InvenController.Instance.SetSpriteToInven(newItemID, 1);
        }
    }

    private void RenewCombTable()
    {
        for (int i = 0; i < mElementList.Count; i++)
        {
            mElementList[i].Renew(
                needNumArr,
                needIDArr
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