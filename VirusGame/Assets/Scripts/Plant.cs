using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //private RespawnObj mRespawn;

    private void Awake()
    {
        //mRespawn = GetComponentInParent<RespawnObj>();
    }

    public void Init()
    {

    }

    public void BeingDestroyed()
    {
        //mRespawn.ChildDestroy();
    }
}
