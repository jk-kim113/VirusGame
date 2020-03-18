using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRangeH : MonoBehaviour
{
    private List<Plant> mFoodList;
    
    private void Awake()
    {
        mFoodList = new List<Plant>();
    }

    public Plant FoundPlant()
    {
        if(mFoodList.Count == 0)
        {
            return null;
        }
        else
        {
            int random = Random.Range(0, mFoodList.Count);

            return mFoodList[random];
        }
    }

    public void ResetList()
    {
        int max = mFoodList.Count;

        for (int i = 0; i < max; i++)
        {
            if (!mFoodList[i].FoodSelected)
            {
                mFoodList.RemoveAt(i);
                i--;
                max--;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("Grass"))
        {
            Plant food = other.gameObject.GetComponent<Plant>();

            if(!food.FoodSelected)
            {
                food.FoodSelected = true;
                mFoodList.Add(food);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Plant"))
        {
            if(mFoodList != null)
            {
                Plant plant = other.gameObject.GetComponent<Plant>();

                if (plant.FoodSelected)
                {
                    plant.FoodSelected = false;

                    ResetList();
                }
            }
        }
    }
}
