using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private float mSpeed;
#pragma warning restore

    private Rigidbody mRB;
    private CharacterController mCHControl;

    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mCHControl = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Player Move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = dir.normalized * mSpeed;
        dir = transform.TransformDirection(dir);
        mCHControl.Move(dir * Time.deltaTime);

        // Player X axis Camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + mouseX, transform.localEulerAngles.z);
    }
}
