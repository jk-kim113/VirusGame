using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGroup : MonoBehaviour
{
    public static DataGroup Instance;

    private Dictionary<int, Sprite> mItemSpriteDic = new Dictionary<int, Sprite>();
    public Dictionary<int, Sprite> ItemSpriteDic { get { return mItemSpriteDic; } }

    private Dictionary<int, int> mItemNumDic = new Dictionary<int, int>();
    public Dictionary<int, int> ItemNumDic { get { return mItemNumDic; } }

    private Dictionary<string, ItemData[]> mItemDataDic = new Dictionary<string, ItemData[]>();
    public Dictionary<string, ItemData[]> ItemDataDic { get { return mItemDataDic; } }

    private Dictionary<int, ItemMakingInfo> mItemMakingInfoDic = new Dictionary<int, ItemMakingInfo>();
    public Dictionary<int, ItemMakingInfo> ItemMakingInfoDic { get { return mItemMakingInfoDic; } }

    private Dictionary<int, FoodMenu> mFoodMenuDic = new Dictionary<int, FoodMenu>();
    public Dictionary<int, FoodMenu> FoodMenuDic { get { return mFoodMenuDic; } }

    private Dictionary<int, FoodMakeType> mFoodMakeTypeDic = new Dictionary<int, FoodMakeType>();
    public Dictionary<int, FoodMakeType> FoodMakeTypeDic { get { return mFoodMakeTypeDic; } }

    private ItemData[] mItemDataArr;
    
    private ItemMakingInfo[] mItemMakingInfoArr;
    private FoodMenu[] mFoodMenuArr;
    public FoodMenu[] FoodMenuArr { get { return mFoodMenuArr; } }
    private FoodMakeType[] mFoodMakeTypeArr;

    private bool bLoaded;
    public bool Loaded { get { return bLoaded; } }

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

        bLoaded = false;
    }

    private void Start()
    {
        JsonDataLoader.Instance.LoadJsonData<ItemData>(out mItemDataArr, "JsonFiles/ItemData");
        JsonDataLoader.Instance.LoadJsonData<ItemMakingInfo>(out mItemMakingInfoArr, "JsonFiles/CombinationData");
        JsonDataLoader.Instance.LoadJsonData<FoodMenu>(out mFoodMenuArr, "JsonFiles/FoodMenu");
        JsonDataLoader.Instance.LoadJsonData<FoodMakeType>(out mFoodMakeTypeArr, "JsonFiles/FoodMakeTypeData");

        Sprite[] GrassSpriteArr = Resources.LoadAll<Sprite>("Sprites/GrassItem");
        Sprite[] TreeSpriteArr = Resources.LoadAll<Sprite>("Sprites/TreeItem");
        Sprite[] FoodSpriteArr = Resources.LoadAll<Sprite>("Sprites/CombItem");

        ItemData[] GrassItemDataArr = new ItemData[GrassSpriteArr.Length];
        ItemData[] TreeItemDataArr = new ItemData[TreeSpriteArr.Length];

        for (int i = 0; i < mItemDataArr.Length; i++)
        {
            if (CheckItemType(mItemDataArr[i].ID) == 1)
            {
                mItemSpriteDic.Add(mItemDataArr[i].ID, GrassSpriteArr[IdToIndex(mItemDataArr[i].ID)]);
                GrassItemDataArr[IdToIndex(mItemDataArr[i].ID)] = mItemDataArr[i];
            }
            else if (CheckItemType(mItemDataArr[i].ID) == 2)
            {
                mItemSpriteDic.Add(mItemDataArr[i].ID, TreeSpriteArr[IdToIndex(mItemDataArr[i].ID)]);
                TreeItemDataArr[IdToIndex(mItemDataArr[i].ID)] = mItemDataArr[i];
            }
        }

        mItemDataDic.Add("Grass", GrassItemDataArr);
        mItemDataDic.Add("Tree", TreeItemDataArr);

        // Set Item Num
        for (int i = 0; i < mItemDataArr.Length; i++)
        {
            mItemNumDic.Add(mItemDataArr[i].ID, 0);
        }
        for(int i = 0; i < mFoodMenuArr.Length; i++)
        {
            for(int j =0; j < mFoodMenuArr[i].TargetID.Length; j++)
            {
                mItemNumDic.Add(mFoodMenuArr[i].TargetID[j], 0);
            }
        }

        // Set Food Sprite
        for(int i = 0; i < mFoodMenuArr.Length; i++)
        {
            mFoodMenuDic.Add(mFoodMenuArr[i].ID, mFoodMenuArr[i]);
            mItemSpriteDic.Add(mFoodMenuArr[i].ID, FoodSpriteArr[i]);

            for (int j = 0; j < mFoodMenuArr[i].TargetID.Length; j++)
            {   
                mItemSpriteDic.Add(mFoodMenuArr[i].TargetID[j], FoodSpriteArr[i]);
            }
        }

        for (int i = 0; i < mItemMakingInfoArr.Length; i++)
        {
            mItemMakingInfoDic.Add(mItemMakingInfoArr[i].TargetID, mItemMakingInfoArr[i]);
        }

        for (int i = 0; i < mFoodMakeTypeArr.Length; i++)
        {
            mFoodMakeTypeDic.Add(mFoodMakeTypeArr[i].TargetID, mFoodMakeTypeArr[i]);
        }

        bLoaded = true;
    }

    public void SetItemNumber(int originalID, int value)
    {
        mItemNumDic[originalID] += value;

        InvenController.Instance.RenewInven(originalID);
    }

    private int IdToIndex(int originalID)
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

    private int CheckItemType(int originalID)
    {
        string originalStr = originalID.ToString();
        char[] originalCharArr = originalStr.ToCharArray();

        return int.Parse(originalCharArr[0].ToString());
    }
}