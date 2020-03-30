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
    private Text mNameText;
    [SerializeField]
    private Text mContentsText;
    [SerializeField]
    private GameObject[] mNeedsArr;
    [SerializeField]
    private Button mMakingBtn;
#pragma warning restore

    public void Init(Sprite mainSprite, string name, string content, int[] needsNum, int[] needsID)
    {
        mMainImg.sprite = mainSprite;
        mNameText.text = name;
        mContentsText.text = content;

        for(int i = 0; i < mNeedsArr.Length; i++)
        {
            if(needsNum[i] > 0)
            {
                mNeedsArr[i].SetActive(true);

                Image needImg = mNeedsArr[i].GetComponentInChildren<Image>();
                needImg.sprite = DataGroup.Instance.ItemSpriteDic[needsID[i]];

                Text num = mNeedsArr[i].GetComponentInChildren<Text>();
                num.text = "x "+ needsNum[i].ToString();

                if(needsNum[i] < DataGroup.Instance.ItemNumDic[needsID[i]])
                {
                    num.color = Color.blue;
                }
                else
                {
                    num.color = Color.red;
                }
            }
            else
            {
                mNeedsArr[i].SetActive(false);
            }
        }
    }
}
