using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRangeC : MonoBehaviour
{
    private Carnivore mCarnivore;
    private Herbivore mFood = null;
    public Herbivore Food { get { return mFood; } set { mFood = value; } }

    public void Init(Carnivore carnivore)
    {
        mCarnivore = carnivore;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Herbivore"))
        {
            if(mFood == null)
            {
                mFood = other.gameObject.GetComponent<Herbivore>();

                if(mCarnivore != null)
                {
                    mCarnivore.SetMovePattern(eBehaviorPattern.Eat);
                }
                else
                {
                    mFood = null;
                }
            }
        }
    }
}
