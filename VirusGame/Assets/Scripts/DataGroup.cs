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

    private Dictionary<int, eItemType> mItemTypeDic = new Dictionary<int, eItemType>();
    public Dictionary<int, eItemType> ItemTypeDic { get { return mItemTypeDic; } }

    private ItemData[] mItemDataArr;
    
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
        
        Sprite[] GrassSpriteArr = Resources.LoadAll<Sprite>("Sprites/GrassItem");
        Sprite[] TreeSpriteArr = Resources.LoadAll<Sprite>("Sprites/TreeItem");
        
        Sprite[] EquipSpriteArr = Resources.LoadAll<Sprite>("Sprites/EquipItem");
        
        ItemData[] GrassItemDataArr = new ItemData[GrassSpriteArr.Length];
        ItemData[] TreeItemDataArr = new ItemData[TreeSpriteArr.Length];

        mItemSpriteDic.Add(5000, EquipSpriteArr[0]);

        // Drop Item Setting
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

            mItemTypeDic.Add(mItemDataArr[i].ID, eItemType.Drop);
        }

        mItemDataDic.Add("Grass", GrassItemDataArr);
        mItemDataDic.Add("Tree", TreeItemDataArr);

        // Set Item Num
        for (int i = 0; i < mItemDataArr.Length; i++)
        {
            mItemNumDic.Add(mItemDataArr[i].ID, 0);
        }
        mItemNumDic.Add(5000, 0);

        bLoaded = true;
    }

    public void SetItemNumber(int originalID, int value)
    {
        if (!mItemNumDic.ContainsKey(originalID))
            mItemNumDic.Add(originalID, 0);

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