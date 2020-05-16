using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYMove : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject mHead;
#pragma warning restore

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
