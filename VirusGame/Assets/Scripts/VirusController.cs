using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
	public static VirusController Instance;

    private VirusData[] mVirusDataArr;
    public VirusData[] VirusDataArr { get { return mVirusDataArr; } }
    private int mVirusRank;

    private Dictionary<int, VirusData> mVirusDataDic = new Dictionary<int, VirusData>();
    public Dictionary<int, VirusData> VirusDataDic { get { return mVirusDataDic; } }

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

        mVirusRank = 1;
        JsonDataLoader.Instance.LoadJsonData<VirusData>(out mVirusDataArr, "JsonFiles/VirusData");

        for(int i = 0; i < mVirusDataArr.Length; i++)
        {
            mVirusDataDic.Add(mVirusDataArr[i].ID, mVirusDataArr[i]);
        }
    }

    public int GetVirusID()
    {
        List<VirusData> data = new List<VirusData>();

        for(int i = 0; i < mVirusDataArr.Length; i++)
        {
            if(mVirusDataArr[i].Rank == mVirusRank)
            {
                data.Add(mVirusDataArr[i]);
            }
        }

        int rand = Random.Range(0, data.Count);

        return data[rand].ID;
    }
}
