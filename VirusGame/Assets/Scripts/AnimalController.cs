using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eHerbivoreKind
{
    Goat,
    Cattle
}

public enum eCarnivoreKind
{
    Bear
}

public class AnimalController : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Herbivore")]
    [SerializeField]
    private HerbivorePool mHerbivorePool;
    [SerializeField]
    private int[] mHerbivoreCount;
    [SerializeField]
    private BoxCollider[] mMoveBoundaryH;

    [Header("Carnivore")]
    [SerializeField]
    private CarnivorePool mCarnivorePool;
    [SerializeField]
    private int[] mCarnivoreCount;
    [SerializeField]
    private BoxCollider[] mMoveBoundaryC;
#pragma warning restore

    private void Start()
    {
        for(int i = 0; i < mHerbivoreCount.Length; i++)
        {
            for(int j = 0; j < mHerbivoreCount[i]; j++)
            {
                mHerbivorePool.SpawnPos = MakeSpawnPos(mMoveBoundaryH[i]);
                Herbivore herbivore = mHerbivorePool.GetFromPool(i);
                herbivore.Init(mMoveBoundaryH[i]);
            }
        }

        for(int i = 0; i < mCarnivoreCount.Length; i++)
        {
            for(int j = 0; j < mCarnivoreCount[i]; j++)
            {
                mCarnivorePool.SpawnPos = MakeSpawnPos(mMoveBoundaryC[i]);
                Carnivore carnivore = mCarnivorePool.GetFromPool(i);
                carnivore.Init(mMoveBoundaryC[i]);
            }
        }
    }

    private Vector3 MakeSpawnPos(BoxCollider coll)
    {
        float xCord = Random.Range(coll.gameObject.transform.position.x - (coll.size.x / 4),
                                    coll.gameObject.transform.position.x + (coll.size.x / 4));
        float zCord = Random.Range(coll.gameObject.transform.position.z - (coll.size.z / 4),
                                    coll.gameObject.transform.position.z + (coll.size.z / 4));

        return new Vector3(xCord, 0, zCord);
    }
}
