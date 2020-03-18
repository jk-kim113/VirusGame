using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    private Rigidbody mRB;
    private Player mPlayer;
    private const float mForcePower = 10.0f;
    private const float mDistance = 2.5f;
    private int mItemID;
    private Renderer mRend;
    private string mItemType;

    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mRend = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        mPlayer = null;
    }

    public void InitObj(int originalId, int rare, string tag)
    {
        mItemID = originalId;
        mItemType = tag;

        switch(rare)
        {
            case 0:
                mRend.material.SetColor("_Color", Color.black);
                break;
            case 1:
                mRend.material.SetColor("_Color", Color.green);
                break;
            case 2:
                mRend.material.SetColor("_Color", Color.blue);
                break;
            case 3:
                mRend.material.SetColor("_Color", Color.magenta);
                break;
            case 4:
                mRend.material.SetColor("_Color", Color.yellow);
                break;
            default:
                break;
        }
    }

    public void ShowItem(Vector3 itemPos)
    {
        transform.position = itemPos + new Vector3(0, 3, 0);
        
        mRB.AddForce(Random.insideUnitSphere * 80);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(mPlayer == null)
            {
                mPlayer = other.gameObject.GetComponent<Player>();
                if(!InvenController.Instance.CheckIsFull(mItemID))
                {
                    StartCoroutine(MoveItemToPlayer());
                }
            }
        }
    }

    private IEnumerator MoveItemToPlayer()
    {
        WaitForFixedUpdate term = new WaitForFixedUpdate();
        Vector3 force = mPlayer.transform.position - transform.position;

        while(Vector3.Magnitude(mPlayer.transform.position - transform.position) > mDistance)
        {
            yield return term;
            force = (mPlayer.transform.position - transform.position) * mForcePower;
            mRB.AddForce(force.x, force.y, force.z);
        }

        gameObject.SetActive(false);
        InvenController.Instance.SetSpriteToInven(mItemID, mItemType);
    }
}
