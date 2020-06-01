using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal
{
#pragma warning disable 0649
    [SerializeField]
    private BoxCollider mAttackCollider;
#pragma warning restore

    private bool isAttack;

    private List<Herbivore> mFoodList = new List<Herbivore>();

    private void Start()
    {
        if(mAttackCollider != null)
        {
            OnOffAttackCollider(false);
        }
        
        isAttack = false;
    }

    private void OnOffAttackCollider(bool value)
    {
        mAttackCollider.gameObject.SetActive(value);
    }

    protected override void Eat()
    {
        mFoodList.Clear();

        RaycastHit[] hitarr = Physics.SphereCastAll(transform.position, 3.0f, Vector3.up, 0f);
        for (int i = 0; i < hitarr.Length; i++)
        {
            if (hitarr[i].collider.CompareTag("Herbivore"))
            {
                mFoodList.Add(hitarr[i].collider.GetComponent<Herbivore>());
            }
        }

        if(mFoodList.Count > 0)
        {
            int selectedID = Random.Range(0, mFoodList.Count);
            Herbivore food = mFoodList[selectedID];
            StartCoroutine(Chasing(food));
        }
        else
        {
            bIsEating = false;
            return;
        }
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
        yield return new WaitForSeconds(2.3f);
        mAnimator.SetBool(StaticValue.ATTACK, false);
        mAnimator.SetBool(StaticValue.EAT, true);
        yield return new WaitForSeconds(4.0f);
        mAnimator.SetBool(StaticValue.EAT, false);
        mNav.speed = 3.5f;
        bIsEating = false;
    }

    protected override void Damage()
    {
        if (isAttack)
            return;
        
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        isAttack = true;
        WaitForFixedUpdate term = new WaitForFixedUpdate();

        Transform target = Player.Instance.transform;
        transform.LookAt(target);

        mNav.stoppingDistance = 5;
        mNav.speed = 7;

        mAnimator.SetBool(StaticValue.WALK, false);
        mAnimator.SetBool(StaticValue.RUN, true);
        mNav.isStopped = false;

        while (Vector3.Distance(transform.position, target.position) > 6.0f)
        {
            transform.LookAt(target);
            mNav.SetDestination(target.position);
            yield return term;
        }
        mAnimator.SetBool(StaticValue.RUN, false);
        mAnimator.SetBool(StaticValue.ATTACK, true);
        OnOffAttackCollider(true);
        mNav.isStopped = true;
        yield return new WaitForSeconds(2.3f);
        OnOffAttackCollider(false);
        mNav.isStopped = false;
        mAnimator.SetBool(StaticValue.ATTACK, false);
        mNav.speed = 3.5f;
        mNav.stoppingDistance = 0;
        bIsEating = false;
        isAttack = false;
    }

}
