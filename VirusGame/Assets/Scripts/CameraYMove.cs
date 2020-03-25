using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYMove : MonoBehaviour
{
    void Update()
    {
        if(!Player.Instance.IsStopMove)
        {
            // Camera Y axis Rotation
            float mouseY = Input.GetAxis("Mouse Y");
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - mouseY, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}
