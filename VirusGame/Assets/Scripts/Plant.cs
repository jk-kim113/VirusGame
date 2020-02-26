using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private int mID;
    private RespawnPlant mRespawn;
    private Transform mTransform;
    private float mGrowPeriod = 5.0f;
    private Coroutine mSpendGrowPeriodCoroutine;

    private void Awake()
    {
        mRespawn = GetComponentInParent<RespawnPlant>();
        mTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        GrowUpPlant(ePlantGrowthType.Early);
    }

    private void Start()
    {
        GrowUpPlant(ePlantGrowthType.LastPeriod);
    }

    public void Init(int id)
    {
        mID = id;
    }

    public void GrowUpPlant(ePlantGrowthType type)
    {
        switch(type)
        {
            case ePlantGrowthType.Early:
                
                mTransform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                mSpendGrowPeriodCoroutine = StartCoroutine(SpendGrowPeriod(ePlantGrowthType.MidTerm));

                break;
            case ePlantGrowthType.MidTerm:
                
                mTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                mSpendGrowPeriodCoroutine = StartCoroutine(SpendGrowPeriod(ePlantGrowthType.LastPeriod));

                break;
            case ePlantGrowthType.LastPeriod:

                mTransform.localScale = new Vector3(1f, 1f, 1f);
                StopCoroutine(mSpendGrowPeriodCoroutine);

                break;
            default:
                break;
        }
    }

    private IEnumerator SpendGrowPeriod(ePlantGrowthType type)
    {
        yield return new WaitForSeconds(mGrowPeriod);

        GrowUpPlant(type);
    }
    
    public void BeingDestroyed()
    {   
        mRespawn.ChildDestroy(mID);
    }
}

public enum ePlantGrowthType
{
    Early,
    MidTerm,
    LastPeriod
}