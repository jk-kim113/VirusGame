using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public static AnimalController Instance;

#pragma warning disable 0649
    [SerializeField]
    private HerbivorePool mHerbivorePool;
    [SerializeField]
    private CarnivorePool mCarnivorePool;
    [SerializeField]
    private BoxCollider[] mMoveBoundary;
    [SerializeField]
    private BloodPool mBloodPool;
#pragma warning restore

    private AnimalData[] mAnimalDataArr;
    private List<Virus> mVirusList = new List<Virus>();
    private float mVirusSpawnPeriod = 5.0f;
    private int mInfectNumber = 0;
    public int InfenctNumber { get { return mInfectNumber; } set { mInfectNumber = value; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
                        herbivore.Init(
                            mMoveBoundary[i],
                            mAnimalDataArr[i].HungerMax,
                            mAnimalDataArr[i].HungerDecrease,
                            mAnimalDataArr[i].HP);
                        mVirusList.Add(herbivore);
                    }
                    break;
                case eAnimalKind.Carnivore:
                    for (int j = 0; j < mAnimalDataArr[i].InitNumber; j++)
                    {
                        mCarnivorePool.SpawnPos = MakeSpawnPos(mMoveBoundary[i]);
                        Carnivore carnivore = mCarnivorePool.GetFromPool(index);
                        carnivore.Init(
                            mMoveBoundary[i],
                            mAnimalDataArr[i].HungerMax,
                            mAnimalDataArr[i].HungerDecrease,
                            mAnimalDataArr[i].HP);
                        mVirusList.Add(carnivore);
                    }
                    break;
                default:
                    Debug.LogError("Wrong AnimalKind");
                    break;
            }
        }
    }

    private void Update()
    {
        if(mInfectNumber == 0)
        {
            mVirusSpawnPeriod -= Time.deltaTime;
            if (mVirusSpawnPeriod <= 0)
            {
                int rand = Random.Range(0, mVirusList.Count);

                Debug.Log("Infect");
                mVirusSpawnPeriod = 5.0f;
                mVirusList[rand].Infect(VirusController.Instance.GetVirusID());
                mInfectNumber++;
            }
        }
    }

    public void ShowVirusMap(int originalID)
    {
        for(int i = 0; i < mVirusList.Count; i++)
        {
           if(mVirusList[i].VirusID == originalID)
            {
                mVirusList[i].ShowVirusMap(true);
            }
        }
    }

    public void Bleed(Vector3 pos, eAnimalDeathType type)
    {
        Blood blood = mBloodPool.GetFromPool((int)type);
        blood.transform.position = pos;

        // Add VirusInfo in the Blood
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
