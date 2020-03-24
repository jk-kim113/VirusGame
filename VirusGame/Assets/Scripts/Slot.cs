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

    private int mItemID;
    public int ItemID { get { return mItemID; } }

    private UIDrag mUIDrag;
    private int mItemNum;

    private void Awake()
    {
        mItemImg.enabled = false;
        mItemNumText.enabled = false;
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

        mItemID = originalID;
        mItemImg.sprite = img;
        mItemNum = num;

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

        RectTransform invPanel = mUIDrag.gameObject.transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            if (invPanel.CompareTag("Slot"))
            {
                Debug.Log(invPanel.gameObject.name);
                Slot slot = invPanel.gameObject.GetComponent<Slot>();
                slot.Init(mItemImg.sprite, mItemNum, mItemID);
            }
        }
    }

    
}
