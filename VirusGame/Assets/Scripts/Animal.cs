using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum eBehaviorPattern
{
    Idle,
    Move,
    Eat,
    Die
}

public enum eAnimalGrowthType
{
    Baby,
    Adult,
    Old,

    max
}

public class Animal : MonoBehaviour
{
    private BoxCollider mMoveBoundary;
    private Coroutine mMovePattern;
    private NavMeshAgent mNav;
    private Animator mAnimator;

    public static readonly int WALK = Animator.StringToHash("IsWalk");

    private void Awake()
    {
        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
    }

    private Vector3 RandomCoordinates()
    {
        float xCord = Random.Range(mMoveBoundary.transform.position.x + (mMoveBoundary.size.x / 2), mMoveBoundary.transform.position.x - (mMoveBoundary.size.x / 2));
        float zCord = Random.Range(mMoveBoundary.transform.position.z + (mMoveBoundary.size.z / 2), mMoveBoundary.transform.position.z - (mMoveBoundary.size.z / 2));

        return new Vector3(xCord, 0, zCord);
    }

    public void Init(BoxCollider boundary)
    {
        mMoveBoundary = boundary;

        int randomNum = Random.Range(0, (int)eAnimalGrowthType.max);
        SetGrowthType(randomNum);

        transform.position = RandomCoordinates();

        mMovePattern = StartCoroutine(MovePattern());
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
    }

    private IEnumerator MovePattern()
    {
        WaitForSeconds term = new WaitForSeconds(3f);
        eBehaviorPattern pattern = eBehaviorPattern.Idle;

        while(true)
        {
            yield return term;

            int probability = Random.Range(0, 2);

            if(probability == 0)
            {
                pattern = eBehaviorPattern.Idle;
            }
            else if(probability == 1)
            {
                pattern = eBehaviorPattern.Move;
            }

            switch(pattern)
            {
                case eBehaviorPattern.Idle:

                    mAnimator.SetBool(WALK, false);
                    mNav.isStopped = true;

                    break;
                case eBehaviorPattern.Move:

                    mNav.isStopped = false;
                    mNav.SetDestination(RandomCoordinates());
                    mAnimator.SetBool(WALK, true);


                    break;
                case eBehaviorPattern.Eat:
                    break;
                case eBehaviorPattern.Die:
                    break;
                default:
                    break;
            }
        }
    }
}
