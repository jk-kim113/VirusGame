using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : Virus
{
    private BoxCollider mMoveBoundary;
    protected NavMeshAgent mNav;
    protected Animator mAnimator;
    private const float mGrowPeriod = 720.0f;
    
    private Coroutine mSpendGrowPeriodCoroutine;

    private BoxCollider mCollider;
    protected bool bIsEating;
    private float mHungerCurrent;
    private float mHungerMax;
    public float HungerMax { set { mHungerMax = value; } }
    private float mHungerDeacrease;
    private float mImmunity { get { return ImmunityCoefficient(mGrowthType) * (mHungerCurrent / mHungerMax * 100); } }
    private bool bIsLoaded;
    private float mIncubationPeriod;
    private float mSpreadPeriod;
    private eAnimalGrowthType mGrowthType;
    private eAnimalDeathType mAnimalDeathType;

    private float mHPmax;
    private float mHPcurrent;

    protected bool bIsAlive;
    private float mMoveTime = 0;

    private eBehaviorPattern mBehaviorPattern = eBehaviorPattern.Idle;

    private float mVirusTimeCheck = 0;

    protected override void Awake()
    {
        base.Awake();

        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<BoxCollider>();
        bIsEating = false;
        bIsLoaded = false;
        mAnimalDeathType = eAnimalDeathType.Natural;

        bIsAlive = true;
    }

    private void OnEnable()
    {
        mCollider.enabled = true;
        mHPcurrent = mHPmax;
    }

    private void Update()
    {
        if(IngameManager.Instance.CurrentGameState == IngameManager.eGameState.PlayGame)
        {
            if (bIsLoaded)
            {
                mHungerCurrent -= mHungerDeacrease * Time.deltaTime;
            }

            if (bIsInfect)
            {
                mVirusTimeCheck += Time.deltaTime;

                if (mIncubationPeriod <= mVirusTimeCheck)
                {
                    mVirusTimeCheck = 0;
                    if (!CheckCurable())
                    {
                        SpreadVirus();
                    }
                }
            }
        }

        if (bIsAlive)
        {
            mMoveTime += Time.deltaTime;
            if (mMoveTime >= 2)
            {
                mMoveTime = 0;
                MovePattern();
            }
        }
    }

    private Vector3 RandomCoordinates()
    {   
        float xCord = Random.Range(mMoveBoundary.gameObject.transform.position.x - (mMoveBoundary.size.x /2), 
                                    mMoveBoundary.gameObject.transform.position.x + (mMoveBoundary.size.x / 2));
        float zCord = Random.Range(mMoveBoundary.gameObject.transform.position.z - (mMoveBoundary.size.z / 2), 
                                    mMoveBoundary.gameObject.transform.position.z + (mMoveBoundary.size.z / 2));

        return new Vector3(xCord, 0, zCord);
    }

    public void Init(BoxCollider boundary, float hungermax, float hungerdeacrease, float hpmax)
    {
        mMoveBoundary = boundary;
        mHungerMax = mHungerCurrent = hungermax;
        mHungerDeacrease = hungerdeacrease;
        mHPmax = mHPcurrent = hpmax;

        bIsLoaded = true;

        int randomNum = Random.Range(0, (int)eAnimalGrowthType.max);
        SetGrowthType(randomNum);
    }

    private void SetGrowthType(int random)
    {
        mGrowthType = (eAnimalGrowthType)(random);

        switch(mGrowthType)
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

    private void MovePattern()
    {
        if (!bIsEating && bIsAlive)
        {
            int maxRan = 0;

            if (mHungerCurrent < mHungerMax * 0.6)
            {
                maxRan = 3;
            }
            else
            {
                maxRan = 2;
            }

            int probability = Random.Range(0, maxRan);


            if (probability == 0)
            {
                mBehaviorPattern = eBehaviorPattern.Idle;
            }
            else if (probability == 1)
            {
                mBehaviorPattern = eBehaviorPattern.Move;
            }
            else if (probability == 2)
            {
                mBehaviorPattern = eBehaviorPattern.Eat;
            }

            SetMovePattern(mBehaviorPattern);
        }
    }

    public void SetMovePattern(eBehaviorPattern pattern)
    {   
        switch (pattern)
        {
            case eBehaviorPattern.Idle:

                mNav.isStopped = true;
                mAnimator.SetBool(StaticValue.WALK, false);

                break;
            case eBehaviorPattern.Move:

                mNav.isStopped = false;
                mNav.SetDestination(RandomCoordinates());
                mAnimator.SetBool(StaticValue.WALK, true);

                break;
            case eBehaviorPattern.Eat:

                bIsEating = true;
                Eat();

                break;
            case eBehaviorPattern.Damage:

                bIsEating = true; //TODO Change value name
                Damage();

                break;
            case eBehaviorPattern.Die:

                mNav.isStopped = true;
                
                mAnimator.SetTrigger(StaticValue.DIE);
                mCollider.enabled = false;
                
                StopCoroutine(mSpendGrowPeriodCoroutine);
                StartCoroutine(Die());
                bIsAlive = false;
                mNav.enabled = false;

                break;
            default:
                Debug.LogError("Wrong Behavior Pattern : " + pattern);
                break;
        }
    }

    protected virtual void Eat()
    {

    }

    protected virtual void Damage()
    {

    }

    private IEnumerator Die()
    {
        AnimalController.Instance.Bleed(this.transform.position, mAnimalDeathType, mVirusID);
        yield return new WaitForSeconds(360.0f);
        
        gameObject.SetActive(false);
    }

    public override void Infect(int id)
    {
        base.Infect(id);

        mAnimalDeathType = eAnimalDeathType.Virus;
        mIncubationPeriod = VirusController.Instance.VirusDataDic[id].IncubationPeriod;
    }

    private void SpreadVirus()
    {
        // colliderEnter를 사용할 경우에는 1 frame 씩 밀리므로 SphereCast를 사용하는것이 좋다.
        RaycastHit[] hitarr = Physics.SphereCastAll(transform.position, 1.5f, Vector3.up, 0f);

        for (int i = 0; i < hitarr.Length; i++)
        {
            if (hitarr[i].collider.CompareTag("Grass")
                || hitarr[i].collider.CompareTag("Tree")
                || hitarr[i].collider.CompareTag("Carnivore")
                || hitarr[i].collider.CompareTag("Herbivore"))
            {
                Virus virus = hitarr[i].collider.gameObject.GetComponent<Virus>();
                virus.Infect(mVirusID);
            }
        }
    }

    public void CureVirusBySyringe()
    {
        if(mVirusID > 0)
        {
            float curableNum = Random.Range(0, 1) * 100;
            if(curableNum < VirusController.Instance.VirusDataDic[mVirusID].CureProbability)
            {
                mVirusID = -999;
                GameObject effect = Resources.Load("Effects/VirusCureEffect") as GameObject;
                effect.transform.position = transform.position;
            }
        }
    }

    private bool CheckCurable()
    {
        float rand = Random.Range(0, 100f);

        if(rand <= mImmunity)
        {
            bIsInfect = false;
            mAnimalDeathType = eAnimalDeathType.Natural;
            AnimalController.Instance.InfenctNumber -= 1;
            mVirusID = -999;

            return true;
        }

        return false;
    }

    private float ImmunityCoefficient(eAnimalGrowthType type)
    {
        switch(type)
        {
            case eAnimalGrowthType.Baby:
                return 0.3f;
            case eAnimalGrowthType.Adult:
                return 0.9f;
            case eAnimalGrowthType.Old:
                return 0.6f;
            default:
                Debug.LogError("Wrong type");
                return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(bIsAlive)
        {
            if (other.CompareTag("Weapon"))
            {
                Timer effect = IngameManager.Instance.GetEffect(eEffectType.HitAnimal);
                effect.transform.position = other.transform.position;
                mHPcurrent -= 50;
                if (mHPcurrent <= 0)
                {
                    SetMovePattern(eBehaviorPattern.Die);
                }
                else
                {
                    SetMovePattern(eBehaviorPattern.Damage);
                }
            }
        }
    }
}
