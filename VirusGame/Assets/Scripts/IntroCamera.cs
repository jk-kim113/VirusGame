using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject mLookAtTarget;
#pragma warning restore

    private void Update()
    {
        transform.RotateAround(mLookAtTarget.transform.position, Vector3.up, 360 * Time.deltaTime * 0.05f);
        transform.LookAt(mLookAtTarget.transform);
    }
}
