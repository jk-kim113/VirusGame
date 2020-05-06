﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

#pragma warning disable 0649
    [SerializeField]
    private float mSpeed;
    [SerializeField]
    private DetectPlayerAction mDetectAction;
    [SerializeField]
    private EffectPool mEffectPool;
#pragma warning restore

    private CharacterController mCHControl;

    private bool bPlantAction;
    public bool IsPlantAction { get { return bPlantAction; } set { bPlantAction = value; } }

    private bool bIsOpenInven;
    public bool IsOpenInven { set { bIsOpenInven = value; } }

    private bool bIsOpenCombTable;
    public bool IsOpenCombTable { set { bIsOpenCombTable = value; } }

    private bool bIsOpenAnalysisTable;
    public bool IsOpenAnalysisTable { set { bIsOpenAnalysisTable = value; } }

    private bool bIsOpenDrugMaker;
    public bool IsOpenDrugMaker { set { bIsOpenDrugMaker = value; } }

    private bool bStopMove;
    public bool IsStopMove { get { return bStopMove; } }

    private float mStaminaMax;
    private float mStaminaCurrent;

    private float mHungryMax;
    private float mHungryCurrent;

    private int mVirusID;

    private bool bIsInfect;

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

        mCHControl = GetComponent<CharacterController>();

        bPlantAction = false;
        bIsOpenInven = false;
        bIsOpenCombTable = false;
        bIsOpenAnalysisTable = false;
        bIsOpenDrugMaker = false;
        bStopMove = false;
        bIsInfect = false;

        mStaminaMax = 100f;
        mStaminaCurrent = mStaminaMax;

        mHungryMax = 100f;
        mHungryCurrent = mHungryMax;
    }

    private void Start()
    {
        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);
    }

    void Update()
    {
        if (!bStopMove)
        {
            // Player Move
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(horizontal, 0, vertical);
            dir = dir.normalized * mSpeed;
            dir = transform.TransformDirection(dir);
            mCHControl.Move(dir * Time.fixedDeltaTime);

            // Player X axis Camera rotation
            float mouseX = Input.GetAxis("Mouse X");
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y + mouseX,
                transform.localEulerAngles.z);
        }

        // Start Plant Action
        if (Input.GetMouseButtonDown(0))
        {
            if(bPlantAction)
            {   
                StartCoroutine(DoingAction());
            }
        }

        // Player Hungry decrease
        mHungryCurrent -= 0.5f * Time.deltaTime;
        MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);

        // Open Inven Box
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(bIsOpenInven)
            {
                OpenInvenBox(true);
                bIsOpenInven = false;
            }
            else if(bIsOpenCombTable)
            {
                OpenCombTable(true);
                bIsOpenCombTable = false;
            }
            else if(bIsOpenAnalysisTable)
            {
                OpenAnalsisTable(true);
                bIsOpenAnalysisTable = false;
            }
            else if(bIsOpenDrugMaker)
            {
                OpenDrugMaker(true);
                bIsOpenDrugMaker = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenInvenBox(false);
            OpenCombTable(false);
            OpenAnalsisTable(false);
            OpenDrugMaker(false);
        }
    }

    private IEnumerator DoingAction()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        Plant plantDetected = mDetectAction.DetectObj.GetComponent<Plant>();
        plantDetected.StartFadeOut();

        Timer effect = mEffectPool.GetFromPool(0);
        effect.gameObject.SetActive(true);
        effect.transform.position = plantDetected.transform.position;

        float max = 100f;
        float current = max;

        plantDetected.SetFadeValue(max, current);

        MainUIController.Instance.OnOffActionGaugeBar(true);
        MainUIController.Instance.ShowActionGaugeBar(max, current);

        while (bPlantAction && (current > 0))
        {
            yield return term;

            current -= 5f;

            plantDetected.SetFadeValue(max, current);

            MainUIController.Instance.ShowActionGaugeBar(max, current);
        }

        if(current <= 0)
        {
            // Get Item from target Obj
            plantDetected.BeingDestroyed();
            InvenController.Instance.SpawnItem(
                plantDetected.gameObject.transform.position,
                plantDetected.gameObject.tag,
                plantDetected.GrowthType,
                plantDetected.VirusID);

            mStaminaCurrent -= 3;
            
            if(mStaminaCurrent <= 0)
            {
                GameOver();
            }
        }
        else
        {
            plantDetected.IsFadeOut = false;
            plantDetected.OnOffOutline(false);
        }

        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.OnOffActionGaugeBar(false);
    }

    private void OpenInvenBox(bool value)
    {
        InvenController.Instance.OpenInvenBox(value);
        bStopMove = value;
    }

    private void OpenCombTable(bool value)
    {
        CombinationController.Instance.OpenCombTable(value);
        bStopMove = value;
    }

    private void OpenAnalsisTable(bool value)
    {
        AnalysisController.Instance.OnOffAnalysisObj(value);
        bStopMove = value;
    }

    private void OpenDrugMaker(bool value)
    {
        DrugMakerController.Instance.OpenDrugMaker(value);
        bStopMove = value;
    }

    public void UseItem(eUseTarget target, float value, int virusID)
    {
        switch(target)
        {
            case eUseTarget.Stamina:
                break;
            case eUseTarget.Hunger:

                mHungryCurrent += value;
                if(mHungryCurrent >= mHungryMax)
                {
                    mHungryCurrent = mHungryMax;
                }

                break;
            case eUseTarget.HP:
                break;
            default:
                break;
        }

        if(virusID > 0 && !bIsInfect)
        {
            mVirusID = virusID;
            Invoke("Infect", VirusController.Instance.VirusDataDic[virusID].IncubationPeriod);
        }
    }

    private void Infect()
    {
        mStaminaMax *= 0.5f;

        if(mStaminaMax <= mStaminaCurrent)
        {
            mStaminaCurrent = mStaminaMax;
        }

        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
    }

    private void GameOver()
    {
        // Game Over!!
    }
}
