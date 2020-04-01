using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal
{
#pragma warning disable 0649
    [SerializeField]
    private DetectRangeH mDetectRange;
#pragma warning restore

    private Vector3 mPlantPosition;

    protected override void Eat()
    {
        Plant plant = mDetectRange.FoundPlant();

        if(plant == null)
        {
            bIsEating = false;
            return;
        }

        mPlantPosition = plant.gameObject.transform.position;

        mNav.SetDestination(mPlantPosition);
        transform.LookAt(mPlantPosition);
        mAnimator.SetBool(StaticValue.WALK, true);

        StartCoroutine(EatPlant(plant));
    }

    private IEnumerator EatPlant(Plant plant)
    {
        WaitForSeconds term = new WaitForSeconds(0.1f);

        while(!mNav.SetDestination(mPlantPosition))
        {
            yield return term;
        }

        mAnimator.SetBool(StaticValue.WALK, false);
        mAnimator.SetBool(StaticValue.EAT, true);

        yield return new WaitForSeconds(4f);

        bIsEating = false;
        mAnimator.SetBool(StaticValue.EAT, false);
        plant.BeingDestroyed();
        mDetectRange.ResetList();
    }
}
