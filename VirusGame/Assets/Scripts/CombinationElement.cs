using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationElement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mMainImg;
    [SerializeField]
    private Text mName;
    [SerializeField]
    private Text mContents;
    [SerializeField]
    private GameObject[] mNeedsArr;
    [SerializeField]
    private Button mMakingBtn;
#pragma warning restore

    public void Init()
    {

    }
}
