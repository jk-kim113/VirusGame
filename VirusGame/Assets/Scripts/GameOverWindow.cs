using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Text mRecordText;
#pragma warning restore

    public void OpenGameOverWindow(float recordTime)
    {
        mRecordText.text = string.Format("");
    }
}
