using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerAction : MonoBehaviour
{
    private float mDectectRange = 20.0f;
    private GameObject mDetectObj;
    public GameObject DetectObj { get { return mDetectObj; } }

    void Update()
    {
        // Ray to distinguish Object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, mDectectRange))
        {
            mDetectObj = hit.collider.gameObject;

            if (hit.collider.CompareTag("Grass") || hit.collider.CompareTag("Tree"))
            {   
                PlantAction(true);
            }
            else if(hit.collider.CompareTag("InventoryBox"))
            {
                OpenInventoryBox(true);
            }
            else
            {
                PlantAction(false);
                OpenInventoryBox(false);
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
        MainUIController.Instance.OnOffActionText(value, "Press 'F' to Open");
        Player.Instance.IsOpenInven = value;
    }
}
