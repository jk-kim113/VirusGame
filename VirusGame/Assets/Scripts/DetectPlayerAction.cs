using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerAction : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private LayerMask mMask;
#pragma warning restore

    private float mDectectRange = 15.0f;
    private GameObject mDetectObj;
    public GameObject DetectObj { get { return mDetectObj; } }

    private Dictionary<string, GameObject> mDetectDic = new Dictionary<string, GameObject>();

    private Plant mPlantObj;
    private int mCount;

    private List<GameObject> mDetectList = new List<GameObject>();

    private void FixedUpdate()
    {
        RaycastHit[] hitArr = Physics.RaycastAll(transform.position, transform.forward, mDectectRange, mMask);
        int plantNum = 0;
        List<Plant> plantList = new List<Plant>();

        for (int i = 0; i < hitArr.Length; i++)
        {
            mDetectList.Add(hitArr[i].collider.gameObject);
        }

        mCount++;

        if (mCount < 4)
        {
            return;
        }

        mCount = 0;

        for (int i = 0; i < mDetectList.Count; i++)
        {
            if (mDetectList[i].CompareTag("Grass") || mDetectList[i].CompareTag("Tree"))
            {
                plantNum++;
                plantList.Add(mDetectList[i].GetComponent<Plant>());
            }
        }

        mDetectList.Clear();

        if (plantNum > 0)
        {
            float minDis = float.MaxValue;
            int minID = 0;

            for (int i = 0; i < plantList.Count; i++)
            {
                float dis = Vector3.Distance(transform.position, plantList[i].transform.position);

                if (minDis > dis)
                {
                    minID = i;
                }
            }

            if (mPlantObj != null)
            {
                if (mPlantObj.ID == plantList[minID].ID)
                {
                    return;
                }
                else
                {
                    mPlantObj.OnOffOutline(false);
                    mPlantObj = null;
                }
            }

            mPlantObj = plantList[minID];
            mDetectObj = mPlantObj.gameObject;
            PlantAction(true);
            mPlantObj.OnOffOutline(true);
        }
        else
        {
            if (mPlantObj != null)
            {
                mPlantObj.OnOffOutline(false);
                mPlantObj = null;
            }

            PlantAction(false);
        }
    }

    void Update()
    {
        // Ray to distinguish Object
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, mDectectRange))
        {
            mDetectObj = hit.collider.gameObject;

            if (hit.collider.CompareTag("InventoryBox"))
            {
                OpenInventoryBox(true);
            }
            else if (hit.collider.CompareTag("CombinationTable"))
            {
                OpenCombinationTable(true);
            }
            else if (hit.collider.CompareTag("AnalysisTable"))
            {
                OpenAnalysisObj(true);
            }
            else if (hit.collider.CompareTag("DrugMaker"))
            {
                OpenDrugMaker(true);
            }
            else
            {
                OpenInventoryBox(false);
                OpenCombinationTable(false);
                OpenAnalysisObj(false);
                OpenDrugMaker(false);
            }
        }
    }

    private void PlantAction(bool value)
    {
        Player.Instance.IsPlantAction = value;
    }

    private void OpenInventoryBox(bool value)
    {
        MainUIController.Instance.OnOffActionText(value, "Press 'F' to Open Inventory Box");
        Player.Instance.IsOpenInven = value;
    }

    private void OpenCombinationTable(bool value)
    {
        MainUIController.Instance.OnOffActionText(value, "Press 'F' to Open Combination Table");
        Player.Instance.IsOpenCombTable = value;
    }

    private void OpenAnalysisObj(bool value)
    {
        MainUIController.Instance.OnOffActionText(value, "Press 'F' to Open AnalysisTable");
        Player.Instance.IsOpenAnalysisTable = value;
    }

    private void OpenDrugMaker(bool value)
    {
        MainUIController.Instance.OnOffActionText(value, "Press 'F' to Open DrugMaker");
        Player.Instance.IsOpenDrugMaker = value;
    }
}
