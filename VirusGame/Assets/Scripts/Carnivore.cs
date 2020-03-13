using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal
{
#pragma warning disable 0649
    [SerializeField]
    private DetectRangeC mDetectRange;
#pragma warning restore

    private void Start()
    {
        mDetectRange.Init(this);
    }

    protected override void Eat()
    {
        Herbivore food = mDetectRange.Food;
        
        if(food == null)
        {
            bIsEating = false;
            return;
        }

        mDetectRange.gameObject.SetActive(false);
        StartCoroutine(Chasing(food));
    }

    private IEnumerator Chasing(Herbivore target)
    {
        WaitForSeconds term = new WaitForSeconds(0.1f);
        transform.LookAt(target.transform.position);
        mNav.speed = 7;
        mNav.isStopped = false;
        mNav.SetDestination(target.transform.position);
        mAnimator.SetBool(AnimatorHash.RUN, true);

        while(Vector3.Magnitude(transform.position - target.transform.position) > 5.0f)
        {   
            mNav.SetDestination(target.transform.position);
            yield return term;
        }

        mNav.isStopped = true;
        mAnimator.SetBool(AnimatorHash.RUN, false);
        mAnimator.SetBool(AnimatorHash.ATTACK, true);
        target.SetMovePattern(eBehaviorPattern.Die);
        mDetectRange.Food = null;
        yield return new WaitForSeconds(2.3f);
        mAnimator.SetBool(AnimatorHash.ATTACK, false);
        mAnimator.SetBool(AnimatorHash.EAT, true);
        yield return new WaitForSeconds(4.0f);
        mAnimator.SetBool(AnimatorHash.EAT, false);
        mNav.speed = 3.5f;
        bIsEating = false;
        mDetectRange.gameObject.SetActive(true);
    }
}
