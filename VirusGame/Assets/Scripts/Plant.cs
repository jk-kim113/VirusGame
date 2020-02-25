using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private int mID;
    private RespawnPlant mRespawn;

    private void Awake()
    {
        mRespawn = GetComponentInParent<RespawnPlant>();
    }

    public void Init(int id)
    {
        mID = id;
    }

    public void BeingDestroyed()
    {   
        mRespawn.ChildDestroy(mID);
    }
}
