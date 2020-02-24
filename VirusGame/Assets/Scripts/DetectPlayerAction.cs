using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerAction : MonoBehaviour
{
    private float mDectectRange = 50.0f;
    private GameObject mDetectObj;
    public GameObject DetectObj { get { return mDetectObj; } }

    void Update()
    {
        // Ray to distinguish Object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, mDectectRange))
        {
            mDetectObj = hit.collider.gameObject;

            if (hit.collider.CompareTag("Plant"))
            {   
                PlantAction(true);
            }
            else
            {
                PlantAction(false);
            }
        }
    }

    private void PlantAction(bool value)
    {
        MainUIController.Instance.OnOffActionText(value);
        Player.Instance.IsPlantAction = value;
    }
}
