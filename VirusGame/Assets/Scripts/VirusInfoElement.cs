using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusInfoElement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mVirusImg;
    [SerializeField]
    private Text mName;
    [SerializeField]
    private GaugeBar mVirusGaugeBar;
    [SerializeField]
    private Text mPercent;
#pragma warning restore

    public void Init(Sprite img, string name, float percent)
    {
        mVirusImg.sprite = img;
        mName.text = name;
        
        Renew(percent);
    }

    public void Renew(float percent)
    {
        mPercent.text = string.Format("{0}%", percent);
        mVirusGaugeBar.ShowGaugeBar(percent);
    }
}
