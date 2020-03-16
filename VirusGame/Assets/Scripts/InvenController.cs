using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour
{
    public static InvenController Instance;

    Dictionary<string, List<Item>> mItemDic = new Dictionary<string, List<Item>>();

    Dictionary<string, Dictionary<ePlantGrowthType, List<int>>> mItemDic2 = new Dictionary<string, Dictionary<ePlantGrowthType, List<int>>>();

#pragma warning disable 0649
    [SerializeField]
    private ItemObjPool mItemObjPool;
#pragma warning restore

    private Item[] mItemArr;

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

        #region Item Data
        mItemArr = new Item[12];

        mItemArr[0] = new Item();
        mItemArr[0].Name = "풀";
        mItemArr[0].ID = 1000;
        mItemArr[0].Info = "풀에서 일반적으로 얻을 수 있는 아이템";

        mItemArr[1] = new Item();
        mItemArr[1].Name = "썩은 풀";
        mItemArr[1].ID = 1001;
        mItemArr[1].Info = "썩은 풀에서 일반적으로 얻을 수 있는 아이템";

        mItemArr[2] = new Item();
        mItemArr[2].Name = "싱싱한 풀";
        mItemArr[2].ID = 1002;
        mItemArr[2].Info = "풀에서 확률적으로 얻을 수 있는 아이템";

        mItemArr[3] = new Item();
        mItemArr[3].Name = "산삼";
        mItemArr[3].ID = 1003;
        mItemArr[3].Info = "풀에서 희귀한 확률로 얻을 수 있는 아이템";

        mItemArr[4] = new Item();
        mItemArr[4].Name = "싱싱한 산삼";
        mItemArr[4].ID = 1004;
        mItemArr[4].Info = "풀에서 매우 희귀한 확률로 얻을 수 있는 아이템";

        mItemArr[5] = new Item();
        mItemArr[5].Name = "100년 묵은 산삼";
        mItemArr[5].ID = 1005;
        mItemArr[5].Info = "전설로만 내려오는 아이템";

        mItemArr[6] = new Item();
        mItemArr[6].Name = "나무가지";
        mItemArr[6].ID = 2000;
        mItemArr[6].Info = "나무에서 일반적으로 얻을 수 있는 아이템";

        mItemArr[7] = new Item();
        mItemArr[7].Name = "썩은 나무가지";
        mItemArr[7].ID = 2001;
        mItemArr[7].Info = "썩은 나무에서 일반적으로 얻을 수 있는 아이템";

        mItemArr[8] = new Item();
        mItemArr[8].Name = "싱싱한 나무가지";
        mItemArr[8].ID = 2002;
        mItemArr[8].Info = "나무에서 확률적으로 얻을 수 있는 아이템";

        mItemArr[9] = new Item();
        mItemArr[9].Name = "수액";
        mItemArr[9].ID = 2003;
        mItemArr[9].Info = "나무에서 희귀한 확률로 얻을 수 있는 아이템";

        mItemArr[10] = new Item();
        mItemArr[10].Name = "싱싱한 수액";
        mItemArr[10].ID = 2004;
        mItemArr[10].Info = "나무에서 매우 희귀한 확률로 얻을 수 있는 아이템";

        mItemArr[11] = new Item();
        mItemArr[11].Name = "100년 묵은 수액";
        mItemArr[11].ID = 2005;
        mItemArr[11].Info = "전설로만 내려오는 아이템";
        #endregion
    }

    private void Start()
    {
        MakeItemGroup();

        

        List<Item> itemList = new List<Item>();

        for(int i = 0; i < mItemArr.Length; i++)
        {
            if(mItemArr[i].ID >= 1000 && mItemArr[i].ID < 2000)
            {
                itemList.Add(mItemArr[i]);
            }
        }

        mItemDic.Add("Grass", itemList);
    }

    private void MakeItemGroup()
    {
        Dictionary<ePlantGrowthType, List<int>> plantItemgroup = new Dictionary<ePlantGrowthType, List<int>>();

        List<int> itemgroup = new List<int>();
        itemgroup.Add(0);

        plantItemgroup.Add(ePlantGrowthType.Early, itemgroup);
        itemgroup.Clear();

        itemgroup.Add(0);
        itemgroup.Add(2);
        itemgroup.Add(3);

        plantItemgroup.Add(ePlantGrowthType.MidTerm, itemgroup);
        itemgroup.Clear();

        itemgroup.Add(0);
        itemgroup.Add(2);
        itemgroup.Add(3);
        itemgroup.Add(4);
        itemgroup.Add(5);

        plantItemgroup.Add(ePlantGrowthType.LastPeriod, itemgroup);
        itemgroup.Clear();

        itemgroup.Add(1);

        plantItemgroup.Add(ePlantGrowthType.Rotten, itemgroup);

        mItemDic2.Add("Grass", plantItemgroup);
    }

    public void SpawnItem(Vector3 Itempos, string tag, ePlantGrowthType type)
    {
        List<Item> itemList = new List<Item>();
        itemList = mItemDic[tag];

        // Early - 풀 0
        // MidTerm - 풀, 싱싱한풀, 산삼 0 2 3
        // LastPeriod - 풀, 싱싱한풀, 산삼, 싱싱한 산삼, 100년 묵은 산삼 0 2 3 4 5
        // Rotten - 썩은 풀 1

        switch(type)
        {
            case ePlantGrowthType.Early:
                
                break;
            case ePlantGrowthType.MidTerm:
                break;
            case ePlantGrowthType.LastPeriod:
                break;
            case ePlantGrowthType.Rotten:
                break;
            default:
                break;
        }


        int itemNum = Random.Range(3, 6);
        for(int i = 0; i < itemNum; i++)
        {
            ItemObj item = mItemObjPool.GetFromPool(0);
            item.ShowItem(Itempos);
        }
    }
}

public class Item
{
    public string Name;
    public int ID;
    public string Info;
}