using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Transform mInsideObj;
#pragma warning restore

    private Vector3 mOriginSize;

    private void Awake()
    {
        mOriginSize = mInsideObj.localScale;
        mInsideObj.localScale = Vector3.zero;
    }

    public void ShowInside(float value)
    {
        Debug.Log(value);
        Vector3 originPos = mInsideObj.localPosition;
        mInsideObj.localScale = new Vector3(mOriginSize.x, value, mOriginSize.z);
        mInsideObj.localPosition = new Vector3(originPos.x, (value / 2) - (mOriginSize.y / 2), originPos.z);
    }
}
