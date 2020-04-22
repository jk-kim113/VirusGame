using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject mVirusIcon;
#pragma warning restore

    protected bool bIsInfect;
    protected int mVirusID;
    public int VirusID { get { return mVirusID; } }

    protected virtual void Awake()
    {
        mVirusID = -999;
        bIsInfect = false;
        mVirusIcon.SetActive(false);
    }

    public virtual void Infect(int id)
    {
        mVirusID = id;
        bIsInfect = true;
    }

    public void ShowVirusMap(bool value)
    {
        mVirusIcon.SetActive(value);
    }
}
