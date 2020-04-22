﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalysisController : MonoBehaviour
{
    public static AnalysisController Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mAnalysisObj;
    [SerializeField]
    private VirusInfoElement mVirusInfoElement;
    [SerializeField]
    private Transform mScrollTarget;
    [SerializeField]
    private Slot mSlot;
    [SerializeField]
    private Button mAnalysisBtn;
    [SerializeField]
    private Button mDisinfectionBtn;
#pragma warning restore

    private int mItemID;

    private Dictionary<int, float> mVirusAnalysisRateDic = new Dictionary<int, float>();
    private Dictionary<int, VirusInfoElement> mElementDic = new Dictionary<int, VirusInfoElement>();

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

        mAnalysisBtn.onClick.AddListener(() => { AnalysisVirus(); });
        mDisinfectionBtn.onClick.AddListener(() => { Disinfection(); });
    }

    private void Start()
    {
        for(int i = 0; i < VirusController.Instance.VirusDataArr.Length; i++)
        {
            mVirusAnalysisRateDic.Add(VirusController.Instance.VirusDataArr[i].ID, 0);
        }
    }

    private void Init()
    {
        for (int i = 0; i < VirusController.Instance.VirusDataArr.Length; i++)
        {
            VirusInfoElement element = Instantiate(mVirusInfoElement, mScrollTarget);
            element.Init(null, VirusController.Instance.VirusDataArr[i].Name, 0);
            mElementDic.Add(VirusController.Instance.VirusDataArr[i].ID, element);
        }
    }

    public void OnOffAnalysisObj(bool value)
    {
        mAnalysisObj.SetActive(value);

        //Init();
    }

    public void GetItem(int originalID)
    {
        // UI

        mItemID = originalID;
    }

    private void AnalysisVirus()
    {
        for(int i = 0; i < InvenController.Instance.InvenVirusInfoDic[mItemID].Count; i++)
        {
            int virusID = InvenController.Instance.InvenVirusInfoDic[mItemID][i];

            if (virusID > 0)
            {
                mVirusAnalysisRateDic[virusID] += VirusController.Instance.VirusDataDic[virusID].AnalysisRate;
                mElementDic[virusID].Renew(mVirusAnalysisRateDic[virusID]);
            }
        }

        mSlot.Renew(0);

        DataGroup.Instance.SetItemNumber(mItemID, 0);
    }

    private void Disinfection()
    {
        for (int i = 0; i < InvenController.Instance.InvenVirusInfoDic[mItemID].Count; i++)
        {
            InvenController.Instance.InvenVirusInfoDic[mItemID][i] = -999;
        }

        mSlot.Renew(0);

        InvenController.Instance.RenewInven(mItemID);
    }
}