using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorePool : ObjPool<Carnivore>
{
    private Vector3 mSapwnPos;
    public Vector3 SpawnPos { set { mSapwnPos = value; } }
    private readonly Quaternion mRotPos = new Quaternion(0, 0, 0, 0);

    protected override Carnivore GetNewObj(int id)
    {
        Carnivore newObj = Instantiate(mOriginArr[id], mSapwnPos, mRotPos);
        mPool[id].Add(newObj);
        return newObj;
    }
}
