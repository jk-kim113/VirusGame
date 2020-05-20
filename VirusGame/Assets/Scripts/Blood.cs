using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private float mBloodAmount;
    public float BloodAmount { get { return mBloodAmount; } }

    private void Awake()
    {
        mBloodAmount = Random.Range(0, 0.5f);
    }
}
