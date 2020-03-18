using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour
{
    public static InvenController Instance;

    Dictionary<string, List<Item>> mItemDic = new Dictionary<string, List<Item>>();

#pragma warning disable 0649
    [SerializeField]
    private ItemObjPool mItemObjPool;
    [SerializeField]
    private Inven mPlayerInven;
#pragma warning restore

    private Item[] mItemArr;
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
    }

    private void Start()
    {
        JsonDataLoader.Instance.LoadJsonData<Item>(out mItemArr, "JsonFiles/ItemData");

        MakeItemList();
    }

    private void MakeItemList()
    {
        List<Item> Grass = new List<Item>();
        List<Item> Tree = new List<Item>();

        for(int i = 0; i < mItemArr.Length; i++)
        {
            string id = mItemArr[i].ID.ToString();
            char[] idfirst = id.ToCharArray();
            if(idfirst[0] == 1)
            {
                Grass.Add(mItemArr[i]);
            }
            else if(idfirst[0] == 2)
            {
                Tree.Add(mItemArr[i]);
            }
        }

        mItemDic.Add("Grass", Grass);
        mItemDic.Add("Tree", Tree);
    }

    public void SpawnItem(Vector3 Itempos, string tag, ePlantGrowthType type)
    {
        List<Item> itemList = new List<Item>();
        itemList = mItemDic[tag];

        int itemNum = Random.Range(3, 6);

        switch (type)
        {
            case ePlantGrowthType.Early:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(0, mItemArr[0].Rare, tag);
                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.MidTerm:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0); // 0 2 3

                    float probability = Random.value;
                    if(probability <= 0.2)
                    {
                        item.InitObj(3, mItemArr[3].Rare, tag);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(2, mItemArr[2].Rare, tag);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(0, mItemArr[0].Rare, tag);
                    }
                    else
                    {
                        item.InitObj(0, mItemArr[0].Rare, tag);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.LastPeriod:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0); // 0 2 3 4 5

                    float probability = Random.value;
                    if (probability <= 0.01)
                    {
                        item.InitObj(5, mItemArr[5].Rare, tag);
                    }
                    else if (probability <= 0.06)
                    {
                        item.InitObj(4, mItemArr[4].Rare, tag);
                    }
                    else if (probability <= 0.2)
                    {
                        item.InitObj(3, mItemArr[3].Rare, tag);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(2, mItemArr[2].Rare, tag);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(0, mItemArr[0].Rare, tag);
                    }
                    else
                    {
                        item.InitObj(0, mItemArr[0].Rare, tag);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.Rotten:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(1, mItemArr[1].Rare, tag);
                    item.ShowItem(Itempos);
                }
                break;
            default:
                break;
        }
    }

    public void SetSpriteToInven(int id, string tag)
    {
        int itemId = TransformIndex(id);

        switch(tag)
        {
            case "Grass":
                mGrassItemNum[itemId]++;
                mPlayerInven.GetItem(mGrassSpriteArr[itemId], mGrassItemNum[itemId], id);
                break;
            case "Tree":
                mTreeItemNum[itemId]++;
                mPlayerInven.GetItem(mTreeSpriteArr[itemId], mTreeItemNum[itemId], id);
                break;
        }
    }

    private int TransformIndex(int id)
    {
        string num = id.ToString();
        char[] numArr = num.ToCharArray();
        
        if(int.Parse(numArr[3].ToString()) == 0)
        {
            return int.Parse(numArr[4].ToString());
        }

        string index = numArr[3].ToString() + numArr[4].ToString();

        return int.Parse(index);
    }
}

public class Item
{
    public string Name;
    public int ID;
    public string Info;
    public int Rare;
}