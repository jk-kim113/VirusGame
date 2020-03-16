using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    private Rigidbody mRB;
    private Player mPlayer;
    private const float mForcePower = 30.0f;
    private const float mDistance = 2.5f;
    
    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        mPlayer = null;
    }

    public void ShowItem(Vector3 itemPos)
    {
        transform.position = itemPos + new Vector3(0, 3, 0);
        
        mRB.AddForce(Random.insideUnitSphere * 500);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(mPlayer == null)
            {
                mPlayer = other.gameObject.GetComponent<Player>();
                StartCoroutine(MoveItemToPlayer());
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
    }
}
