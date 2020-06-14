using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour
{
    public static InvenController Instance;

#pragma warning disable 0649
    [SerializeField]
    private ItemObjPool mItemObjPool;
    [SerializeField]
    private Inven mPlayerInven;
    [SerializeField]
    private GameObject mInvenBox;
    [SerializeField]
    private UIDrag mUIDrag;
#pragma warning restore

    public UIDrag UIDragImg { get { return mUIDrag; } }

    private Dictionary<eItemType, Dictionary<int, int>> mItemNumberDic = new Dictionary<eItemType, Dictionary<int, int>>();
    public Dictionary<eItemType, Dictionary<int, int>> ItemNumberDic { get { return mItemNumberDic; } }

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

        Dictionary<int, int> itemNumInit = new Dictionary<int, int>();
        itemNumInit.Add(-999, -999);
        mItemNumberDic.Add(eItemType.Drop, itemNumInit);
        mItemNumberDic.Add(eItemType.Use, itemNumInit);
        mItemNumberDic.Add(eItemType.Equip, itemNumInit);
    }

    public void SettingItemNumber(eItemType type, int id, int num)
    {
        if (mItemNumberDic[type].ContainsKey(id))
        {
            mItemNumberDic[type][id] += num;
        }
        else
        {   
            mItemNumberDic[type].Add(id, num);
        }
    }

    public void SpawnPlantItem(Vector3 Itempos, string tag, ePlantGrowthType type)
    {
        int itemNum = Random.Range(3, 6);
        int selectedID = -999;
        float probability = 0;

        for (int i = 0; i < itemNum; i++)
        {
            probability = Random.value;

            switch (type)
            {
                case ePlantGrowthType.Early:

                    selectedID = 0;

                    break;
                case ePlantGrowthType.MidTerm:

                    if (probability <= 0.2)
                    {
                        selectedID = 3;
                    }
                    else if (probability <= 0.5)
                    {
                        selectedID = 2;
                    }
                    else if (probability <= 1.0)
                    {
                        selectedID = 0;
                    }
                    else
                    {
                        selectedID = 0;
                    }

                    break;
                case ePlantGrowthType.LastPeriod:

                    if (probability <= 0.01)
                    {
                        selectedID = 5;
                    }
                    else if (probability <= 0.06)
                    {
                        selectedID = 4;
                    }
                    else if (probability <= 0.2)
                    {
                        selectedID = 3;
                    }
                    else if (probability <= 0.5)
                    {
                        selectedID = 2;
                    }
                    else if (probability <= 1.0)
                    {
                        selectedID = 0;
                    }
                    else
                    {
                        selectedID = 0;
                    }

                    break;
                case ePlantGrowthType.Rotten:

                    selectedID = 1;

                    break;
                default:

                    Debug.LogError("Wrong type : " + type);

                    break;
            }

            ItemObj item = mItemObjPool.GetFromPool(0);
            item.InitObj(
                DataGroup.Instance.DropItemDataDic[selectedID + 1].ID,
                DataGroup.Instance.DropItemDataDic[selectedID + 1].Rare,
                1,
                eItemType.Drop);
            item.ShowItem(Itempos);
        }
    }

    public void SetSpriteToInven(int originalId, int num ,eItemType itemType)
    {
        SettingItemNumber(itemType, originalId, num);
        mPlayerInven.GetItem(originalId, mItemNumberDic[itemType][originalId], itemType);
    }

    public bool CheckIsFull(int originalId)
    {
        return mPlayerInven.CheckIsFull(originalId);
    }

    public void OpenInvenBox(bool value)
    {
        mInvenBox.gameObject.SetActive(value);
    }

    public void DropItem(int originalID, int num)
    {
        for (int i = 0; i < num; i++)
        {
            ItemObj item = mItemObjPool.GetFromPool(0);
            item.DropItem();
            item.InitObj(
                originalID,
                DataGroup.Instance.DropItemDataDic[originalID].Rare,
                1,
                eItemType.Drop);
        }

        SettingItemNumber(eItemType.Drop, originalID, -num);
    }

    public void RenewInven(int originalID, eItemType itemType)
    {
        mPlayerInven.RenewInven(originalID, itemType);
    }
}