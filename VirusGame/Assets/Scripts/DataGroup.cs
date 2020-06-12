using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGroup : MonoBehaviour
{
    public static DataGroup Instance;

    private Dictionary<eItemType, Dictionary<int, Sprite>> mSpriteDataDic = new Dictionary<eItemType, Dictionary<int, Sprite>>();
    private Dictionary<int, DropItemData> mDropItemDataDic = new Dictionary<int, DropItemData>();
    private Dictionary<int, UseItemData> mUseItemDataDic = new Dictionary<int, UseItemData>();
    private Dictionary<int, EquipItemData> mEquipItemDataDic = new Dictionary<int, EquipItemData>();
    private Dictionary<eItemType, Dictionary<int, ItemMakingInfo>> mItemMakingInfoDic = new Dictionary<eItemType, Dictionary<int, ItemMakingInfo>>();

    public Dictionary<eItemType, Dictionary<int, Sprite>> SpriteDataDic { get { return mSpriteDataDic; } }
    public Dictionary<int, DropItemData> DropItemDataDic { get { return mDropItemDataDic; } }
    public Dictionary<int, UseItemData> UseItemDataDic { get { return mUseItemDataDic; } }
    public Dictionary<int, EquipItemData> EquipItemDataDic { get { return mEquipItemDataDic; } }
    public Dictionary<eItemType, Dictionary<int, ItemMakingInfo>> ItemMakingInfoDic { get { return mItemMakingInfoDic; } }

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
        Sprite[] dropItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/DropItem");
        Sprite[] useItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/UseItem");
        Sprite[] equipItemSpriteArr = Resources.LoadAll<Sprite>("Sprites/EquipItem");

        DropItemData[] dropItemDataArr;
        UseItemData[] useItemDataArr;
        EquipItemData[] equipItemDataArr;
        ItemMakingInfo[] itemMakingInfoArr;
        JsonDataLoader.Instance.LoadJsonData<DropItemData>(out dropItemDataArr, "JsonFiles/DropItemData");
        JsonDataLoader.Instance.LoadJsonData<UseItemData>(out useItemDataArr, "JsonFiles/UseItemData");
        JsonDataLoader.Instance.LoadJsonData<EquipItemData>(out equipItemDataArr, "JsonFiles/EquipItemData");
        JsonDataLoader.Instance.LoadJsonData<ItemMakingInfo>(out itemMakingInfoArr, "JsonFiles/ItemMakingInfoData");

        Dictionary<int, Sprite> dropSpriteData = new Dictionary<int, Sprite>();
        Dictionary<int, Sprite> useSpriteData = new Dictionary<int, Sprite>();
        Dictionary<int, Sprite> equipSpriteData = new Dictionary<int, Sprite>();
        for (int i = 0; i < dropItemDataArr.Length; i++)
        {
            dropSpriteData.Add(dropItemDataArr[i].ID, dropItemSpriteArr[i]);
            mDropItemDataDic.Add(dropItemDataArr[i].ID, dropItemDataArr[i]);
        }
        mSpriteDataDic.Add(eItemType.Drop, dropSpriteData);
        for (int i = 0; i < useItemDataArr.Length; i++)
        {
            useSpriteData.Add(useItemDataArr[i].ID, useItemSpriteArr[i]);
            mUseItemDataDic.Add(useItemDataArr[i].ID, useItemDataArr[i]);
        }
        mSpriteDataDic.Add(eItemType.Use, useSpriteData);
        for (int i = 0; i < equipItemDataArr.Length; i++)
        {
            equipSpriteData.Add(equipItemDataArr[i].ID, equipItemSpriteArr[i]);
            mEquipItemDataDic.Add(equipItemDataArr[i].ID, equipItemDataArr[i]);
        }
        mSpriteDataDic.Add(eItemType.Equip, equipSpriteData);

        Dictionary<int, ItemMakingInfo> UseItemMakingInfo = new Dictionary<int, ItemMakingInfo>();
        Dictionary<int, ItemMakingInfo> EquipItemMakingInfo = new Dictionary<int, ItemMakingInfo>();
        for(int i = 0; i < itemMakingInfoArr.Length; i++)
        {
            if (itemMakingInfoArr[i].ItemType == eItemType.Use)
            {
                UseItemMakingInfo.Add(itemMakingInfoArr[i].TargetID, itemMakingInfoArr[i]);
            }
            else if (itemMakingInfoArr[i].ItemType == eItemType.Equip)
            {
                EquipItemMakingInfo.Add(itemMakingInfoArr[i].TargetID, itemMakingInfoArr[i]);
            }
        }

        mItemMakingInfoDic.Add(eItemType.Use, UseItemMakingInfo);
        mItemMakingInfoDic.Add(eItemType.Equip, EquipItemMakingInfo);
        
        bLoaded = true;
    }
}