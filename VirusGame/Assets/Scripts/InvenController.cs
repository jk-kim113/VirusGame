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
    }

    public void SpawnItem(Vector3 Itempos, string tag, ePlantGrowthType type)
    {
        int itemNum = Random.Range(3, 6);

        switch (type)
        {
            case ePlantGrowthType.Early:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][0].ID,
                        DataGroup.Instance.ItemDataDic[tag][0].Rare,
                        1);
                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.MidTerm:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);

                    float probability = Random.value;
                    if(probability <= 0.2)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][3].ID,
                        DataGroup.Instance.ItemDataDic[tag][3].Rare,
                        1);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][2].ID,
                        DataGroup.Instance.ItemDataDic[tag][2].Rare,
                        1);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][1].ID,
                        DataGroup.Instance.ItemDataDic[tag][1].Rare,
                        1);
                    }
                    else
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][0].ID,
                        DataGroup.Instance.ItemDataDic[tag][0].Rare,
                        1);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.LastPeriod:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);

                    float probability = Random.value;
                    if (probability <= 0.01)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][5].ID,
                        DataGroup.Instance.ItemDataDic[tag][5].Rare,
                        1);
                    }
                    else if (probability <= 0.06)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][4].ID,
                        DataGroup.Instance.ItemDataDic[tag][4].Rare,
                        1);
                    }
                    else if (probability <= 0.2)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][3].ID,
                        DataGroup.Instance.ItemDataDic[tag][3].Rare,
                        1);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][2].ID,
                        DataGroup.Instance.ItemDataDic[tag][2].Rare,
                        1);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][0].ID,
                        DataGroup.Instance.ItemDataDic[tag][0].Rare,
                        1);
                    }
                    else
                    {
                        item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][0].ID,
                        DataGroup.Instance.ItemDataDic[tag][0].Rare,
                        1);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.Rotten:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(
                        DataGroup.Instance.ItemDataDic[tag][1].ID,
                        DataGroup.Instance.ItemDataDic[tag][1].Rare,
                        1);
                    item.ShowItem(Itempos);
                }
                break;
            default:
                break;
        }
    }

    public void SetSpriteToInven(int originalId, int num)
    {
        DataGroup.Instance.SetItemNumber(originalId, num);
        mPlayerInven.GetItem(
            DataGroup.Instance.ItemSpriteDic[originalId],
            DataGroup.Instance.ItemNumDic[originalId],
            originalId);
    }

    private int TransformIndex(int originalID)
    {
        string originalStr = originalID.ToString();
        char[] originalCharArr = originalStr.ToCharArray();

        if (int.Parse(originalCharArr[2].ToString()) == 0)
        {
            return int.Parse(originalCharArr[3].ToString());
        }

        string Index = originalCharArr[2].ToString() + originalCharArr[3].ToString();

        return int.Parse(Index);
    }

    private string IdTostring(int originalID)
    {
        string originalStr = originalID.ToString();
        char[] originalCharArr = originalStr.ToCharArray();

        if (int.Parse(originalCharArr[0].ToString()) == 1)
        {
            return "Grass";
        }
        else if (int.Parse(originalCharArr[0].ToString()) == 2)
        {
            return "Tree";
        }
        else
        {
            return "";
        }
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
        ItemObj item = mItemObjPool.GetFromPool(0);
        item.DropItem();
        item.InitObj(
            originalID,
            DataGroup.Instance.ItemDataDic[IdTostring(originalID)][TransformIndex(originalID)].Rare,
            num);
        DataGroup.Instance.SetItemNumber(originalID, -num);
    }
}