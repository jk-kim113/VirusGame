using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrugMakerController : MonoBehaviour
{
    public static DrugMakerController Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mDrugMakerObj;
    [SerializeField]
    private Slot[] mSlotArr;
    [SerializeField]
    private Slot mResultSlot;
    [SerializeField]
    private Button mMakeBtn;
#pragma warning restore

    private List<int> mItemIDList = new List<int>();

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

        mMakeBtn.onClick.AddListener(() => { MakeDrug(); });
    }

    public void OpenDrugMaker(bool value)
    {
        mDrugMakerObj.SetActive(value);
    }

    public void GetItem(int originalID)
    {
        mItemIDList.Add(originalID);
    }

    private void MakeDrug()
    {
        if(mItemIDList.Count == 0)
        {
            return;
        }

        int rareEffi = 0;
        int itemNum = 0;

        for(int i = 0; i < mItemIDList.Count; i++)
        {
            rareEffi += DataGroup.Instance.DropItemDataDic[mItemIDList[i]].Rare;
            itemNum += InvenController.Instance.ItemNumberDic[eItemType.Drop][mItemIDList[i]];

            InvenController.Instance.SettingItemNumber(eItemType.Drop, mItemIDList[i], -itemNum);
        }

        float drugRare = rareEffi * itemNum / 30;

        mResultSlot.Init(null, 1, 1, eItemType.Use);

        for(int i = 0; i < mSlotArr.Length; i++)
        {
            mSlotArr[i].Renew(0);
        }

        mItemIDList.Clear();
    }
}
