using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private HerbivorePool mHerbivorePool;
    [SerializeField]
    private CarnivorePool mCarnivorePool;
    [SerializeField]
    private BoxCollider[] mMoveBoundary;
#pragma warning restore

    private AnimalData[] mAnimalDataArr;

    private void Awake()
    {
        JsonDataLoader.Instance.LoadJsonData<AnimalData>(out mAnimalDataArr, "JsonFiles/AnimalData");
    }

    private void Start()
    {
        for(int i = 0; i < mAnimalDataArr.Length; i++)
        {
            eAnimalKind type = IDToEnum(mAnimalDataArr[i].ID);
            int index = IDToIndex(mAnimalDataArr[i].ID);

            switch(type)
            {
                case eAnimalKind.Herbivore:
                    for (int j = 0; j < mAnimalDataArr[i].InitNumber; j++)
                    {
                        mHerbivorePool.SpawnPos = MakeSpawnPos(mMoveBoundary[i]);
                        Herbivore herbivore = mHerbivorePool.GetFromPool(index);
                        herbivore.Init(mMoveBoundary[i], mAnimalDataArr[i].HungerMax, mAnimalDataArr[i].HungerDecrease);
                    }
                    break;
                case eAnimalKind.Carnivore:
                    for (int j = 0; j < mAnimalDataArr[i].InitNumber; j++)
                    {
                        mCarnivorePool.SpawnPos = MakeSpawnPos(mMoveBoundary[i]);
                        Carnivore carnivore = mCarnivorePool.GetFromPool(index);
                        carnivore.Init(mMoveBoundary[i], mAnimalDataArr[i].HungerMax, mAnimalDataArr[i].HungerDecrease);
                    }
                    break;
                default:
                    Debug.LogError("Wrong AnimalKind");
                    break;
            }
        }
    }

    private eAnimalKind IDToEnum(int originalID)
    {
        string originalStr = originalID.ToString();
        char[] originalCharArr = originalStr.ToCharArray();

        switch(originalCharArr[0])
        {
            case '1':
                return eAnimalKind.Herbivore;
                
            case '2':
                return eAnimalKind.Carnivore;

            default:
                return eAnimalKind.max;
        }
    }

    private int IDToIndex(int originalID)
    {
        string originalStr = originalID.ToString();
        char[] originalCharArr = originalStr.ToCharArray();

        if (int.Parse(originalCharArr[2].ToString()) == 0)
        {
            return int.Parse(originalCharArr[3].ToString());
        }

        string Index = originalCharArr[2].ToString() + originalCharArr[3].ToString();

        return int.Parse(Index);
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
