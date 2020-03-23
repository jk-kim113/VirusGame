using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour
{
    public static InvenController Instance;

    Dictionary<string, List<ItemData>> mItemDic = new Dictionary<string, List<ItemData>>();

#pragma warning disable 0649
    [SerializeField]
    private ItemObjPool mItemObjPool;
    [SerializeField]
    private Inven mPlayerInven;
    [SerializeField]
    private Inven mInvenBox;
#pragma warning restore

    private ItemData[] mItemDataArr;
    private Sprite[] mGrassSpriteArr;
    private Sprite[] mTreeSpriteArr;
    private int[] mGrassItemNum;
    private int[] mTreeItemNum;

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

        mGrassSpriteArr = Resources.LoadAll<Sprite>("Sprites/GrassItem");
        mTreeSpriteArr = Resources.LoadAll<Sprite>("Sprites/TreeItem");

        mGrassItemNum = new int[mGrassSpriteArr.Length];
        for (int i = 0; i < mGrassSpriteArr.Length; i++)
        {
            mGrassItemNum[i] = 0;
        }

        mTreeItemNum = new int[mTreeSpriteArr.Length];
        for (int i = 0; i < mTreeSpriteArr.Length; i++)
        {
            mTreeItemNum[i] = 0;
        }

        JsonDataLoader.Instance.LoadJsonData<ItemData>(out mItemDataArr, "JsonFiles/ItemData");
    }

    private void Start()
    {
        MakeItemList();
    }

    private void MakeItemList()
    {
        List<ItemData> Grass = new List<ItemData>();
        List<ItemData> Tree = new List<ItemData>();

        for(int i = 0; i < mItemDataArr.Length; i++)
        {
            string id = mItemDataArr[i].ID.ToString();
            char[] idfirst = id.ToCharArray();
            if(int.Parse(idfirst[0].ToString()) == 1)
            {
                Grass.Add(mItemDataArr[i]);
            }
            else if(int.Parse(idfirst[0].ToString()) == 2)
            {
                Tree.Add(mItemDataArr[i]);
            }
        }

        mItemDic.Add("Grass", Grass);
        mItemDic.Add("Tree", Tree);
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
                    item.InitObj(mItemDic[tag][0].ID, mItemDataArr[0].Rare, tag);
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
                        item.InitObj(mItemDic[tag][3].ID, mItemDataArr[3].Rare, tag);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(mItemDic[tag][2].ID, mItemDataArr[2].Rare, tag);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDataArr[0].Rare, tag);
                    }
                    else
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDataArr[0].Rare, tag);
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
                        item.InitObj(mItemDic[tag][5].ID, mItemDataArr[5].Rare, tag);
                    }
                    else if (probability <= 0.06)
                    {
                        item.InitObj(mItemDic[tag][4].ID, mItemDataArr[4].Rare, tag);
                    }
                    else if (probability <= 0.2)
                    {
                        item.InitObj(mItemDic[tag][3].ID, mItemDataArr[3].Rare, tag);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(mItemDic[tag][2].ID, mItemDataArr[2].Rare, tag);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDataArr[0].Rare, tag);
                    }
                    else
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDataArr[0].Rare, tag);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.Rotten:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(mItemDic[tag][1].ID, mItemDataArr[1].Rare, tag);
                    item.ShowItem(Itempos);
                }
                break;
            default:
                break;
        }
    }

    public void SetSpriteToInven(int originalId, string tag)
    {
        int itemId = TransformIndex(originalId);

        switch(tag)
        {
            case "Grass":
                mGrassItemNum[itemId]++;
                mPlayerInven.GetItem(mGrassSpriteArr[itemId], mGrassItemNum[itemId], originalId);
                break;
            case "Tree":
                mTreeItemNum[itemId]++;
                mPlayerInven.GetItem(mTreeSpriteArr[itemId], mTreeItemNum[itemId], originalId);
                break;
        }
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

    public bool CheckIsFull(int originalId)
    {
        return mPlayerInven.CheckIsFull(originalId);
    }

    public void OpenInvenBox(bool value)
    {
        mInvenBox.gameObject.SetActive(value);
    }
}