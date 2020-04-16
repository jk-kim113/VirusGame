using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    protected bool bIsInfect;
    protected int mVirusID;
    public int VirusID { get { return mVirusID; } }

    protected virtual void Awake()
    {
        mVirusID = -999;
        bIsInfect = false;
    }

    public virtual void Infect(int id)
    {
        mVirusID = id;
        bIsInfect = true;
    }
}
