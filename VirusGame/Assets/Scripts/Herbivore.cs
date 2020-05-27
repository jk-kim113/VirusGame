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
    private bool isRunning;

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

    protected override void Damage()
    {
        if (isRunning)
            return;

        isRunning = true;
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        WaitForFixedUpdate term = new WaitForFixedUpdate();

        mNav.isStopped = false;
        mAnimator.SetBool(StaticValue.WALK, false);
        mAnimator.SetBool(StaticValue.RUN, true);
        mNav.speed = 10.0f;
        float time = 1.0f;

        while (time > 0)
        {
            time -= Time.fixedDeltaTime;
            mNav.isStopped = false;
            transform.position += transform.forward * 2 * Time.deltaTime;
            yield return term;
        }

        bIsEating = false;
        mAnimator.SetBool(StaticValue.RUN, false);
        mNav.speed = 3.5f;
        isRunning = false;
    }
}
