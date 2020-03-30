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
    private GameObject mInvenBox;
    [SerializeField]
    private UIDrag mUIDrag;
#pragma warning restore

    private ItemData[] mItemDataArr;

    Dictionary<string, int[]> mItemNumDic = new Dictionary<string, int[]>();
    Dictionary<string, Sprite[]> mItemSpriteDic = new Dictionary<string, Sprite[]>();
    
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

        JsonDataLoader.Instance.LoadJsonData<ItemData>(out mItemDataArr, "JsonFiles/ItemData");

        Sprite[] mGrassSpriteArr = Resources.LoadAll<Sprite>("Sprites/GrassItem");
        Sprite[] mTreeSpriteArr = Resources.LoadAll<Sprite>("Sprites/TreeItem");

        mItemSpriteDic.Add("Grass", mGrassSpriteArr);
        mItemSpriteDic.Add("Tree", mTreeSpriteArr);

        int[] GrassItemNum = new int[mItemSpriteDic["Grass"].Length];
        for (int i = 0; i < mItemSpriteDic["Grass"].Length; i++)
        {
            GrassItemNum[i] = 0;
        }

        int[] TreeItemNum = new int[mItemSpriteDic["Tree"].Length];
        for (int i = 0; i < mItemSpriteDic["Tree"].Length; i++)
        {
            TreeItemNum[i] = 0;
        }

        mItemNumDic.Add("Grass", GrassItemNum);
        mItemNumDic.Add("Tree", TreeItemNum);
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
                    item.InitObj(mItemDic[tag][0].ID, mItemDic[tag][0].Rare, tag, 1);
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
                        item.InitObj(mItemDic[tag][3].ID, mItemDic[tag][3].Rare, tag, 1);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(mItemDic[tag][2].ID, mItemDic[tag][2].Rare, tag, 1);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDic[tag][0].Rare, tag, 1);
                    }
                    else
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDic[tag][0].Rare, tag, 1);
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
                        item.InitObj(mItemDic[tag][5].ID, mItemDic[tag][5].Rare, tag, 1);
                    }
                    else if (probability <= 0.06)
                    {
                        item.InitObj(mItemDic[tag][4].ID, mItemDic[tag][4].Rare, tag, 1);
                    }
                    else if (probability <= 0.2)
                    {
                        item.InitObj(mItemDic[tag][3].ID, mItemDic[tag][3].Rare, tag, 1);
                    }
                    else if(probability <= 0.5)
                    {
                        item.InitObj(mItemDic[tag][2].ID, mItemDic[tag][2].Rare, tag, 1);
                    }
                    else if(probability <= 1.0)
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDic[tag][0].Rare, tag, 1);
                    }
                    else
                    {
                        item.InitObj(mItemDic[tag][0].ID, mItemDic[tag][0].Rare, tag, 1);
                    }

                    item.ShowItem(Itempos);
                }
                break;
            case ePlantGrowthType.Rotten:
                for (int i = 0; i < itemNum; i++)
                {
                    ItemObj item = mItemObjPool.GetFromPool(0);
                    item.InitObj(mItemDic[tag][1].ID, mItemDic[tag][1].Rare, tag, 1);
                    item.ShowItem(Itempos);
                }
                break;
            default:
                break;
        }
    }

    public void SetSpriteToInven(int originalId, string tag, int num)
    {
        int itemId = TransformIndex(originalId);

        mItemNumDic[tag][itemId] += num;
        mPlayerInven.GetItem(mItemSpriteDic[tag][itemId], mItemNumDic[tag][itemId], originalId, tag);
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

    public void DropItem(int originalID, int num, string tag)
    {
        ItemObj item = mItemObjPool.GetFromPool(0);
        item.DropItem();
        item.InitObj(mItemDic[tag][TransformIndex(originalID)].ID, mItemDic[tag][TransformIndex(originalID)].Rare, tag, num);
        mItemNumDic[tag][TransformIndex(originalID)] -= num;
    }
}