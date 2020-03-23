using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    private BoxCollider mMoveBoundary;
    protected NavMeshAgent mNav;
    protected Animator mAnimator;
    private const float mGrowPeriod = 720.0f;
    private Coroutine mMovePatternCoroutine;
    private Coroutine mSpendGrowPeriodCoroutine;
    private BoxCollider mCollider;
    protected bool bIsEating;
    private float mHungerCurrent;
    private float mHungerMax;
    public float HungerMax { set { mHungerMax = value; } }

    private void Awake()
    {
        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<BoxCollider>();
        bIsEating = false;

        mHungerMax = mHungerCurrent;
    }

    private void OnEnable()
    {
        mCollider.enabled = true;
    }

    private Vector3 RandomCoordinates()
    {   
        float xCord = Random.Range(mMoveBoundary.gameObject.transform.position.x - (mMoveBoundary.size.x /2), 
                                    mMoveBoundary.gameObject.transform.position.x + (mMoveBoundary.size.x / 2));
        float zCord = Random.Range(mMoveBoundary.gameObject.transform.position.z - (mMoveBoundary.size.z / 2), 
                                    mMoveBoundary.gameObject.transform.position.z + (mMoveBoundary.size.z / 2));

        return new Vector3(xCord, 0, zCord);
    }

    public void Init(BoxCollider boundary)
    {
        mMoveBoundary = boundary;

        int randomNum = Random.Range(0, (int)eAnimalGrowthType.max);
        SetGrowthType(randomNum);
        
        mMovePatternCoroutine = StartCoroutine(MovePattern());
    }

    private void SetGrowthType(int random)
    {
        switch((eAnimalGrowthType)random)
        {
            case eAnimalGrowthType.Baby:

                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                
                break;
            case eAnimalGrowthType.Adult:

                transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                
                break;
            case eAnimalGrowthType.Old:
                
                transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

                break;
            default:
                break;
        }

        mSpendGrowPeriodCoroutine = StartCoroutine(SpendGrowPeriod(random));
    }

    private IEnumerator SpendGrowPeriod(int type)
    {
        yield return new WaitForSeconds(mGrowPeriod);

        if((eAnimalGrowthType)type == eAnimalGrowthType.Old)
        {
            SetMovePattern(eBehaviorPattern.Die);
        }
        else
        {
            type++;
            SetGrowthType(type);
        }
    }

    private IEnumerator MovePattern()
    {   
        WaitForSeconds term = new WaitForSeconds(2f);
        eBehaviorPattern pattern = eBehaviorPattern.Idle;
        SetMovePattern(pattern);
        
        while (true)
        {
            yield return term;

            if(!bIsEating)
            {
                int probability = Random.Range(0, 3);

                if (probability == 0)
                {
                    pattern = eBehaviorPattern.Idle;
                }
                else if (probability == 1)
                {
                    pattern = eBehaviorPattern.Move;
                }
                else if (probability == 2)
                {
                    pattern = eBehaviorPattern.Eat;
                }

                SetMovePattern(pattern);
            }
        }
    }

    public void SetMovePattern(eBehaviorPattern pattern)
    {   
        switch (pattern)
        {
            case eBehaviorPattern.Idle:

                mNav.isStopped = true;
                mAnimator.SetBool(AnimatorHash.WALK, false);

                break;
            case eBehaviorPattern.Move:

                mNav.isStopped = false;
                mNav.SetDestination(RandomCoordinates());
                mAnimator.SetBool(AnimatorHash.WALK, true);

                break;
            case eBehaviorPattern.Eat:

                bIsEating = true;
                Eat();

                break;
            case eBehaviorPattern.Die:

                mNav.isStopped = true;
                mAnimator.SetBool(AnimatorHash.WALK, false);
                mAnimator.SetBool(AnimatorHash.DIE, true);
                mCollider.enabled = false;
                StopCoroutine(mMovePatternCoroutine);
                StopCoroutine(mSpendGrowPeriodCoroutine);
                StartCoroutine(Die());

                break;
            default:
                break;
        }
    }

    protected virtual void Eat()
    {

    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(360.0f);
        
        gameObject.SetActive(false);
    }
}
