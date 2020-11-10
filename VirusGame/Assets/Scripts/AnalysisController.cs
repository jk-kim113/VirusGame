using System.Collections;
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
#pragma warning restore

    private int mItemID;
    private bool isOnSlot;

    private Dictionary<int, float> mAnalysisDic = new Dictionary<int, float>();
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
        isOnSlot = false;
        mAnalysisBtn.interactable = isOnSlot;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(isOnSlot)
        {
            if (mSlot.ItemID < 0)
            {
                isOnSlot = false;
                mAnalysisBtn.interactable = false;
            }
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
    }

    public void GetItem(int originalID, eItemType itemType)
    {
        if (itemType != eItemType.Equip || originalID != 3)
            return;

        if(!isOnSlot)
        {
            isOnSlot = true;
            mAnalysisBtn.interactable = true;
        }
    }

    private void AnalysisVirus()
    {
        foreach(int key in Player.Instance.BeakerInfoDic.Keys)
        {
            if(!mAnalysisDic.ContainsKey(key))
            {
                mAnalysisDic.Add(key, Player.Instance.BeakerInfoDic[key] * VirusController.Instance.VirusDataDic[key].AnalysisRate);
            }
            else
            {
                mAnalysisDic[key] += Player.Instance.BeakerInfoDic[key] * VirusController.Instance.VirusDataDic[key].AnalysisRate;
            }

            mElementDic[key].Renew(mAnalysisDic[key]);
        }

        mSlot.Renew(0);
        Player.Instance.BeakerInfoDic.Clear();
    }
}
