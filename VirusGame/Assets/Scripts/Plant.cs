using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Virus
{
#pragma warning disable 0649
    [SerializeField]
    private Material NonOutlineMtr;
    [SerializeField]
    private Material OutlineMtr;
#pragma warning restore

    private int mID;
    private RespawnPlant mRespawn;
    private Transform mTransform;
    private float mGrowPeriod = 360.0f;
    private Coroutine mSpendGrowPeriodCoroutine;
    private Renderer mRenderer;
    private bool bIsOutline;

    private bool bFoodSelected = false;
    public bool FoodSelected { get { return bFoodSelected; } set { bFoodSelected = value; } }

    private ePlantGrowthType mGrowthType;
    public ePlantGrowthType GrowthType { get { return mGrowthType; } }

    protected override void Awake()
    {
        base.Awake();

        mRespawn = GetComponentInParent<RespawnPlant>();
        mTransform = GetComponent<Transform>();
        mRenderer = GetComponent<Renderer>();
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

    public override void Infect(int id)
    {
        base.Infect(id);
    }

    public void GrowUpPlant(ePlantGrowthType type)
    {
        mGrowthType = type;

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
                mSpendGrowPeriodCoroutine = StartCoroutine(SpendGrowPeriod(ePlantGrowthType.Rotten));

                break;
            case ePlantGrowthType.Rotten:

                mRenderer.material.SetColor("_Color", Color.black);
                StopCoroutine(mSpendGrowPeriodCoroutine);

                break;
            default:
                Debug.LogError("Wrong Plant Type : " + type);
                break;
        }
    }

    private IEnumerator SpendGrowPeriod(ePlantGrowthType type)
    {
        yield return new WaitForSeconds(mGrowPeriod);

        GrowUpPlant(type);
    }
    
    public void OnOffOutline(bool value)
    {
        if(value)
        {
            mRenderer.material = OutlineMtr;
        }
        else
        {
            mRenderer.material = NonOutlineMtr;
        }
        
    }

    public void BeingDestroyed()
    {
        bFoodSelected = false;
        mRespawn.ChildDestroy(mID);
    }
}