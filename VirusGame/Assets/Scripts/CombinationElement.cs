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

    private int mID;
    public int ID { get { return mID; } }

    public void Init(Sprite mainSprite, int originalId, string name, string content, int[] needsNum, int[] needsID,
        StaticValue.OneIntParaCallBack callback)
    {
        mMainImg.sprite = mainSprite;
        mID = originalId;
        mNameText.text = name;
        mContentsText.text = content;
        mMakingBtn.onClick.AddListener(() => { callback(originalId); });

        Renew(needsNum, needsID);
    }

    public void Renew(int[] needsNum, int[] needsID)
    {
        for (int i = 0; i < mNeedsArr.Length; i++)
        {
            if (needsNum[i] > 0)
            {
                mNeedsArr[i].SetActive(true);

                Image needImg = mNeedsArr[i].GetComponentInChildren<Image>();
                needImg.sprite = DataGroup.Instance.SpriteDataDic[eItemType.Drop][needsID[i]];

                Text num = mNeedsArr[i].GetComponentInChildren<Text>();
                num.text = "x " + needsNum[i].ToString();

                if(InvenController.Instance.ItemNumberDic[eItemType.Drop].ContainsKey(needsID[i]))
                {
                    if (needsNum[i] > InvenController.Instance.ItemNumberDic[eItemType.Drop][needsID[i]])
                    {
                        num.color = Color.red;
                    }
                    else
                    {
                        num.color = Color.black;
                    }
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
