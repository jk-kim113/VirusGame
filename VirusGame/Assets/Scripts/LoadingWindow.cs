using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mLoadingBar;
#pragma warning restore

    public void OpenLoadingWnd()
    {
        mLoadingBar.fillAmount = 0;
    }

    public void ShowGaugeBar(float value)
    {
        mLoadingBar.fillAmount = value;
    }
}
