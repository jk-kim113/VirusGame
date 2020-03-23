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
    [SerializeField]
    private GaugeBar mStaminaGaugeBar;
    [SerializeField]
    private GaugeBar mHungryGaugeBar;
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

    public void OnOffActionText(bool value, string text)
    {   
        mActionText.gameObject.SetActive(value);
        mActionText.text = text;
    }

    public void OnOffActionGaugeBar(bool value)
    {
        mActionGaugeBar.gameObject.SetActive(value);
    }

    public void ShowActionGaugeBar(float max, float current)
    {
        mActionGaugeBar.ShowGaugeBar(current / max);
    }

    public void ShowStaminaGaugeBar(float max, float current)
    {
        mStaminaGaugeBar.ShowGaugeBar(current / max);
    }

    public void ShowHungryGaugeBar(float max, float current)
    {
        mHungryGaugeBar.ShowGaugeBar(current / max);
    }
}
