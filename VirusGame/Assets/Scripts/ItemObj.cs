using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    private Rigidbody mRB;
    
    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
    }

    public void ShowItem(Vector3 playerPos, Vector3 itemPos)
    {
        transform.position = itemPos + new Vector3(0, 3, 0);
        Vector3 player = new Vector3(playerPos.x, 0, playerPos.z);
        Vector3 item = new Vector3(itemPos.x, 0, itemPos.z);
        Vector3 force = item - player;
        force.y = 10.0f;
        mRB.AddForce(force * 30);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            mRB.MovePosition(other.gameObject.transform.position);
        }
    }
}
