using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDataLoader : MonoBehaviour
{
    public static JsonDataLoader Instance;

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
    }

    public void LoadJsonData<T>(out T[] output, string location)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(location);
        if (textAsset != null)
        {
            string data = textAsset.text;
            if (!string.IsNullOrEmpty(data))
            {
                output = JsonConvert.DeserializeObject<T[]>(data);
                return;
            }
            else
            {
                Debug.LogWarning("File is empty : " + location);
            }
        }
        else
        {
            Debug.LogWarning("Wrong location or file name : " + location);
        }
        output = null;
    }
}
