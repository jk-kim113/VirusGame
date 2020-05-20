using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Transform mInsideObj;
#pragma warning restore

    private float mBloodAmount;
    private float mOriginScale;

    private bool mFullBlood;
    public bool IsFullBlood { get { return mFullBlood; } }

    private void Start()
    {
        mInsideObj.localScale = Vector3.zero;
        mFullBlood = false;
    }

    public void ShowInside(float value)
    {
        mOriginScale = mInsideObj.localScale.y;
        mBloodAmount += value;
        if(mBloodAmount > 2.0f)
        {
            // 원래 사이즈를 가지고 곱해주
            mBloodAmount = 2.0f;
            mFullBlood = true;
        }

        StartCoroutine(FillInside(mOriginScale, mBloodAmount));

    }

    private IEnumerator FillInside(float start, float end)
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        while(start < end)
        {
            yield return term;
            start += 0.03f;
            mInsideObj.localScale = new Vector3(1, start, 1);
        }

        mInsideObj.localScale = new Vector3(1, end, 1);
    }
}
