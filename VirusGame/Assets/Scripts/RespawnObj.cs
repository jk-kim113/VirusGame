using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObj<T> : MonoBehaviour where T : Component
{
    protected T[] mIncludedObj;
    protected int mChildrenCount;

    protected void Awake()
    {
        mIncludedObj = GetComponentsInChildren<T>();
        mChildrenCount = mIncludedObj.Length;
    }

    public void ChildDestroy()
    {
        mChildrenCount--;
    }
}
