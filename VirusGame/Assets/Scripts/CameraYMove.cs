using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYMove : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Transform mCollectBloodCameraPos;
    [SerializeField]
    private Transform mOriginalPos;
#pragma warning restore

    private bool bIsMove;
    

    public void SetCameraPos(bool value)
    {
        bIsMove = value;
        if (value)
        {
            Player.Instance.BloodInBeaker();
            Invoke("SetOriginalPos", 3.5f);
        }
        else
            transform.position = mOriginalPos.position;
    }

    private void SetOriginalPos()
    {
        Player.Instance.CollectBlood(false);
    }

    void Update()
    {
        if(!Player.Instance.IsStopMove)
        {
            // Camera Y axis Rotation
            float mouseY = Input.GetAxis("Mouse Y");
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x - mouseY,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z);


            if (transform.localEulerAngles.x > 45)
            {
                transform.localEulerAngles = new Vector3(
                45,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z);
            }
        }

        if(bIsMove)
        {
            transform.position = mCollectBloodCameraPos.position;
        }
    }
}
