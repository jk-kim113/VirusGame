using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mGaugeBar;
#pragma warning restore

    public void ShowActionGaugeBar(float value)
    {
        mGaugeBar.fillAmount = value;
    }
}
