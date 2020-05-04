using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerAction : MonoBehaviour
{
    private float mDectectRange = 20.0f;
    private GameObject mDetectObj;
    public GameObject DetectObj { get { return mDetectObj; } }

    private Plant mPlantObj;

    void Update()
    {
        // Ray to distinguish Object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, mDectectRange))
        {
            mDetectObj = hit.collider.gameObject;

            if (hit.collider.CompareTag("Grass") || hit.collider.CompareTag("Tree"))
            {   
                if(mPlantObj != null)
                {
                    mPlantObj.OnOffOutline(false);
                    mPlantObj = null;
                }

                PlantAction(true);
                mPlantObj = hit.collider.gameObject.GetComponent<Plant>();
                mPlantObj.OnOffOutline(true);
            }
            else if(hit.collider.CompareTag("InventoryBox"))
            {
                OpenInventoryBox(true);
            }
            else if(hit.collider.CompareTag("CombinationTable"))
            {
                OpenCombinationTable(true);
            }
            else if(hit.collider.CompareTag("AnalysisTable"))
            {
                OpenAnalysisObj(true);
            }
            else if(hit.collider.CompareTag("DrugMaker"))
            {
                OpenDrugMaker(true);
            }
            else
            {
                if(mPlantObj != null)
                {
                    mPlantObj.OnOffOutline(false);
                    mPlantObj = null;
                }

                PlantAction(false);
                OpenInventoryBox(false);
                OpenCombinationTable(false);
                OpenAnalysisObj(false);
                OpenDrugMaker(false);
            }
        }
    }

    private void PlantAction(bool value)
    {
        MainUIController.Instance.OnOffActionText(value, "채집하기");
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
