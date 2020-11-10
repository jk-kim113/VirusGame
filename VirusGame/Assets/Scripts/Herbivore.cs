using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : Animal
{
    private Vector3 mPlantPosition;
    private bool isRunning;

    private List<Plant> mPlantList = new List<Plant>();

    protected override void Eat()
    {
        mPlantList.Clear();

        RaycastHit[] hitarr = Physics.SphereCastAll(transform.position, 3.0f, Vector3.up, 0f);
        for(int i = 0; i < hitarr.Length; i++)
        {
            if(hitarr[i].collider.CompareTag("Grass"))
            {
                mPlantList.Add(hitarr[i].collider.GetComponent<Plant>());
            }
        }

        if(mPlantList.Count > 0)
        {
            int selectedID = Random.Range(0, mPlantList.Count);

            Plant plant = mPlantList[selectedID];

            mPlantPosition = plant.gameObject.transform.position;

            mNav.SetDestination(mPlantPosition);
            transform.LookAt(mPlantPosition);
            mAnimator.SetBool(StaticValue.WALK, true);

            StartCoroutine(EatPlant(plant));
        }
        else
        {
            bIsEating = false;
            return;
        }
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
            transform.position += transform.forward * 2 * Time.deltaTime;
            yield return term;
        }

        bIsEating = false;
        mAnimator.SetBool(StaticValue.RUN, false);
        mNav.speed = 3.5f;
        isRunning = false;
    }
}
