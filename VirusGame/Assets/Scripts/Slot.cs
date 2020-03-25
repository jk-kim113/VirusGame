using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
#pragma warning disable 0649
    [SerializeField]
    private Image mItemImg;
    [SerializeField]
    private Text mItemNumText;
#pragma warning restore
    public Sprite ItemImg { get { return mItemImg.sprite; } }

    private int mItemID;
    public int ItemID { get { return mItemID; } }
    
    private int mItemNum;
    public int ItemNum { get { return mItemNum; } }

    private bool bIsFull;
    public bool IsFull { get { return bIsFull; } }

    private UIDrag mUIDrag;
    private string mItemTag;
    public string ItemTag { get { return mItemTag; } }

    private void Awake()
    {
        mItemImg.enabled = false;
        mItemNumText.enabled = false;
        bIsFull = false;
        mItemID = -999;
    }

    private void Start()
    {
        mUIDrag = InvenController.Instance.UIDragImg;
    }

    public void Init(Sprite img, int num, int originalID, string tag)
    {
        mItemImg.enabled = true;
        mItemNumText.enabled = true;
        bIsFull = true;

        mItemID = originalID;
        mItemImg.sprite = img;
        mItemNum = num;

        mItemTag = tag;

        if(num == 1)
        {
            mItemNumText.text = "";
        }
        else
        {
            mItemNumText.text = num.ToString();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.pointerEnter = mUIDrag.gameObject;
        mUIDrag.gameObject.transform.position = Input.mousePosition;
        mItemImg.enabled = false;
        mItemNumText.enabled = false;
        mUIDrag.OnImage(mItemImg.sprite);
    }

    public void OnDrag(PointerEventData eventData)
    {
        mUIDrag.gameObject.transform.position = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mUIDrag.OffImage();

        RaycastResult ray = eventData.pointerCurrentRaycast;
        
        if(ray.gameObject != null)
        {
            if (ray.gameObject.CompareTag("Slot"))
            {
                Slot slot = ray.gameObject.GetComponent<Slot>();

                if (slot.IsFull)
                {
                    Sprite tempSprite = slot.ItemImg;
                    int tempNum = slot.ItemNum;
                    int tempID = slot.ItemID;
                    string tempTag = slot.ItemTag;

                    slot.Init(mItemImg.sprite, mItemNum, mItemID, mItemTag);
                    Init(tempSprite, tempNum, tempID, tempTag);


                }
                else
                {
                    slot.Init(mItemImg.sprite, mItemNum, mItemID, mItemTag);
                    mItemID = -999;
                    bIsFull = false;
                }
            }
        }
    }
}
