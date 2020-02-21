using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYMove : MonoBehaviour
{
    private float mDectectRange = 100.0f;

    void Update()
    {
        // Camera Y axis Rotation
        float mouseY = Input.GetAxis("Mouse Y");
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - mouseY, transform.localEulerAngles.y, transform.localEulerAngles.z);

        // Ray to distinguish Object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, mDectectRange))
        {
            if(hit.collider.CompareTag("Plant"))
            {
                Debug.Log("Plant");
            }
        }
    }
}
