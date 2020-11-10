using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private float mBloodAmount;
    public float BloodAmount { get { return mBloodAmount; } }

    private int mVirusID;
    public int VirusID { get { return mVirusID;  } }

    public void InitBlood(int virusID)
    {
        mVirusID = virusID;
        mBloodAmount = Random.Range(0.3f, 0.8f);
    }
}
