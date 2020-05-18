using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
    [SerializeField]
    private Transform mHandPod;

    private void Update()
    {
        transform.position = mHandPod.position;
    }
}
