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

    public void SpawnPlantItem(Vector3 Itempos, string tag, ePlantGrowthType type, int virusID)
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
                DataGroup.Instance.ItemDataDic[tag][selectedID].ID,
                DataGroup.Instance.ItemDataDic[tag][selectedID].Rare,
                1);
            item.ShowItem(Itempos);
        }
    }

    public void SetSpriteToInven(int originalId, int num)
    {
        DataGroup.Instance.SetItemNumber(originalId, num);
        SetPlayerInven(originalId);
    }

    public void SetPlayerInven(int originalID)
    {
        mPlayerInven.GetItem(
            DataGroup.Instance.ItemSpriteDic[originalID],
            DataGroup.Instance.ItemNumDic[originalID],
            originalID);
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
        for (int i = 0; i < num; i++)
        {
            ItemObj item = mItemObjPool.GetFromPool(0);
            item.DropItem();
            item.InitObj(
                originalID,
                DataGroup.Instance.ItemDataDic[IdTostring(originalID)][TransformIndex(originalID)].Rare,
                1);
        }
        
        DataGroup.Instance.SetItemNumber(originalID, -num);
    }

    public void RenewInven(int originalID)
    {
        mPlayerInven.RenewInven(originalID);
    }
}