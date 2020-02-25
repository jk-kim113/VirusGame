using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlant : MonoBehaviour
{
    private Plant[] mIncludedObj;
    
    private void Awake()
    {
        mIncludedObj = GetComponentsInChildren<Plant>();
    }

    private void Start()
    {
        for(int i = 0; i < mIncludedObj.Length; i++)
        {
            mIncludedObj[i].Init(i);
        }
    }

    public void ChildDestroy(int id)
    {
        mIncludedObj[id].gameObject.SetActive(false);
        StartCoroutine(RespawnObj(id));
    }

    private IEnumerator RespawnObj(int id)
    {
        yield return new WaitForSeconds(10);

        mIncludedObj[id].gameObject.SetActive(true);
    }
}
