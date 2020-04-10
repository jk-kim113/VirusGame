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

    public void Init(Sprite img, int num, int originalID)
    {
        mItemImg.enabled = true;
        mItemNumText.enabled = true;
        bIsFull = true;

        mItemID = originalID;
        mItemImg.sprite = img;

        Renew(num);
    }

    public void Renew(int num)
    {
        mItemNum = num;
        if (num == 0)
        {
            mItemImg.enabled = false;
            mItemNumText.enabled = false;
            bIsFull = false;
            mItemID = -999;
        }
        else if (num == 1)
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

                    slot.Init(mItemImg.sprite, mItemNum, mItemID);
                    Init(tempSprite, tempNum, tempID);


                }
                else
                {
                    slot.Init(mItemImg.sprite, mItemNum, mItemID);
                    mItemID = -999;
                    bIsFull = false;
                }
            }
            else
            {
                mItemImg.enabled = true;
                mItemNumText.enabled = true;
                bIsFull = true;
            }
        }
        else
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit))
            {
                if(hit.collider.CompareTag("Ground"))
                {
                    InvenController.Instance.DropItem(mItemID, mItemNum);
                    mItemID = -999;
                    bIsFull = false;
                }
                else
                {
                    mItemImg.enabled = true;
                    mItemNumText.enabled = true;
                    bIsFull = true;
                }
            }
        }
    }
}
