using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAnimaKind
{
    Goat,
    Bear
}

public class AnimalController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private AnimalPool mAnimalPool;
    [SerializeField]
    private int[] mAnimalCountArr;
#pragma warning restore

    private BoxCollider[] mMoveBoundary;

    private void Awake()
    {
        mMoveBoundary = GetComponentsInChildren<BoxCollider>();
    }

    private void Start()
    {
        for(int i = 0; i < mAnimalCountArr.Length; i++)
        {
            for(int j = 0; j < mAnimalCountArr[i]; j++)
            {
                Animal animal = mAnimalPool.GetFromPool(i);
                animal.Init(mMoveBoundary[i]);
            }
        }
    }
}
