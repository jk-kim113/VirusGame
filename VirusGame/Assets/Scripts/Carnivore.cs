using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal
{
#pragma warning disable 0649
    [SerializeField]
    private DetectRangeC mDetectRange;
    [SerializeField]
    private BoxCollider[] mAttackColliderArr;
#pragma warning restore

    private bool isAttack;

    private void Start()
    {
        mDetectRange.Init(this);
        OnOffAttackCollider(false);
    }

    private void OnOffAttackCollider(bool value)
    {
        for(int i = 0; i < mAttackColliderArr.Length; i++)
        {
            mAttackColliderArr[i].enabled = value;
        }
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
        mAnimator.SetBool(StaticValue.RUN, true);

        while(Vector3.Magnitude(transform.position - target.transform.position) > 5.0f)
        {   
            mNav.SetDestination(target.transform.position);
            yield return term;
        }

        mNav.isStopped = true;
        mAnimator.SetBool(StaticValue.RUN, false);
        mAnimator.SetBool(StaticValue.ATTACK, true);
        target.SetMovePattern(eBehaviorPattern.Die);
        mDetectRange.Food = null;
        yield return new WaitForSeconds(2.3f);
        mAnimator.SetBool(StaticValue.ATTACK, false);
        mAnimator.SetBool(StaticValue.EAT, true);
        yield return new WaitForSeconds(4.0f);
        mAnimator.SetBool(StaticValue.EAT, false);
        mNav.speed = 3.5f;
        bIsEating = false;
        mDetectRange.gameObject.SetActive(true);
    }

    protected override void Damage()
    {
        if (isAttack)
            return;
        
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        WaitForFixedUpdate term = new WaitForFixedUpdate();

        Transform target = Player.Instance.transform;
        transform.LookAt(target);

        mNav.speed = 7;
        mNav.isStopped = false;
        mAnimator.SetBool(StaticValue.RUN, true);
        OnOffAttackCollider(true);

        while (Vector3.Distance(transform.position, target.position) > 6.0f)
        {
            transform.LookAt(target);
            mNav.SetDestination(target.position);
            yield return term;
        }
        
        mAnimator.SetBool(StaticValue.RUN, false);
        mAnimator.SetBool(StaticValue.ATTACK, true);
        mNav.isStopped = true;
        yield return new WaitForSeconds(2.3f);
        OnOffAttackCollider(false);
        mNav.isStopped = false;
        mAnimator.SetBool(StaticValue.ATTACK, false);
        mNav.speed = 3.5f;
        bIsEating = false;
    }

}
