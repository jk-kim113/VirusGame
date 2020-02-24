using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public static MainUIController Instance;

#pragma warning disable 0649
    [SerializeField]
    private Text mActionText;
    [SerializeField]
    private GaugeBar mActionGaugeBar;
#pragma warning restore

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mActionText.gameObject.SetActive(false);
    }

    public void OnOffActionText(bool value)
    {   
        mActionText.gameObject.SetActive(value);
    }

    public void OnOffActionGaugeBar(bool value)
    {
        mActionGaugeBar.gameObject.SetActive(value);
    }

    public void ShowActionGaugeBar(float max, float current)
    {
        mActionGaugeBar.ShowActionGaugeBar(current / max);
    }
}
