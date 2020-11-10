using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

#pragma warning disable 0649
    [SerializeField]
    private float mSpeed;
    [SerializeField]
    private DetectPlayerAction mDetectAction;
    [SerializeField]
    private CameraYMove mCameraYMove;
    [SerializeField]
    private Transform[] mEquipPos;
    [SerializeField]
    private GameObject[] mEquipPrefabs;
#pragma warning restore

    private CharacterController mCHControl;

    private bool bPlantAction;
    public bool IsPlantAction { get { return bPlantAction; } set { bPlantAction = value; } }

    private bool bIsOpenInven;
    public bool IsOpenInven { set { bIsOpenInven = value; } }

    private bool bIsOpenCombTable;
    public bool IsOpenCombTable { set { bIsOpenCombTable = value; } }

    private bool bIsOpenAnalysisTable;
    public bool IsOpenAnalysisTable { set { bIsOpenAnalysisTable = value; } }

    private bool bIsOpenDrugMaker;
    public bool IsOpenDrugMaker { set { bIsOpenDrugMaker = value; } }

    private bool bIsCollectBlood;
    public bool IsCollectBlood { set { bIsCollectBlood = value; } }

    private bool bIsOpenEquipMaker;
    public bool IsOpenEquipMaker { set { bIsOpenEquipMaker = value; } }

    private bool bStopMove;
    public bool IsStopMove { get { return bStopMove; } }

    private float mStaminaMax;
    private float mStaminaCurrent;

    private float mHungryMax;
    private float mHungryCurrent;

    private float mHPmax;
    private float mHPcurrent;

    private float mImmunity { get { return (mHungryCurrent / mHungryMax); } }

    private int mVirusID;

    private bool bIsInfect;

    private Animator mAnim;

    private Weapon mWeapon;
    private Beaker mBeaker;
    private Syringe mSyringe;

    private Dictionary<int, float> mBeakerInfoDic = new Dictionary<int, float>();
    public Dictionary<int, float> BeakerInfoDic { get { return mBeakerInfoDic; } }

    private void Awake()
    {   
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mCHControl = GetComponent<CharacterController>();

        bPlantAction = false;
        bIsOpenInven = false;
        bIsOpenCombTable = false;
        bIsOpenAnalysisTable = false;
        bIsOpenDrugMaker = false;
        bIsCollectBlood = false;
        bStopMove = false;
        bIsInfect = false;

        mStaminaMax = 100f;
        mStaminaCurrent = mStaminaMax;

        mHungryMax = 100f;
        mHungryCurrent = mHungryMax;

        mHPmax = mHPcurrent = 100;

        mAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);
        MainUIController.Instance.ShowHPGaugeBar(mHPmax, mHPcurrent);
    }

    private void FixedUpdate()
    {
        if(IngameManager.Instance.CurrentGameState == IngameManager.eGameState.PlayGame)
        {
            if (!bStopMove)
            {
                // Player Move
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Vector3 dir = new Vector3(horizontal, 0, vertical);
                dir = dir.normalized * mSpeed;
                dir = transform.TransformDirection(dir);
                mCHControl.Move(dir * Time.fixedDeltaTime);

                // Player X axis Camera rotation
                float mouseX = Input.GetAxis("Mouse X");
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x,
                    transform.localEulerAngles.y + mouseX,
                    transform.localEulerAngles.z);

                if (dir.magnitude > 0.1)
                {
                    mAnim.SetBool("walk", true);
                }
                else
                {
                    mAnim.SetBool("walk", false);
                }
            }
        }
    }

    void Update()
    {
        if(IngameManager.Instance.CurrentGameState == IngameManager.eGameState.PlayGame)
        {
            // Start Plant Action
            if (Input.GetMouseButtonDown(0))
            {
                if (bPlantAction)
                {
                    StartCoroutine(DoingAction());
                }
            }

            // Player Hungry decrease
            mHungryCurrent -= 0.5f * Time.deltaTime;
            MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);

            // Open Inven Box
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (bIsOpenInven)
                {
                    OpenInvenBox(true);
                    bIsOpenInven = false;
                }
                else if (bIsOpenCombTable)
                {
                    OpenUseItemMaker(true);
                    bIsOpenCombTable = false;
                }
                else if (bIsOpenAnalysisTable)
                {
                    OpenAnalsisTable(true);
                    bIsOpenAnalysisTable = false;
                }
                else if (bIsOpenDrugMaker)
                {
                    OpenDrugMaker(true);
                    bIsOpenDrugMaker = false;
                }
                else if (bIsCollectBlood)
                {
                    CollectBlood(true);
                    bIsCollectBlood = false;
                }
                else if (bIsOpenEquipMaker)
                {
                    OpenEquipItemMaker(true);
                    bIsOpenEquipMaker = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenInvenBox(false);
                OpenUseItemMaker(false);
                OpenAnalsisTable(false);
                OpenDrugMaker(false);
                CollectBlood(false);
                OpenEquipItemMaker(false);
            }

            if (mWeapon != null)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    mAnim.SetBool("attack01", true);
                    mWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    mAnim.SetBool("attack01", false);
                    mWeapon.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
            }

            if (mSyringe != null)
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(mDetectAction.transform.position, mDetectAction.transform.forward, out hit, 7.0f))
                    {
                        if(hit.collider.CompareTag("Herbivore") || hit.collider.CompareTag("Carnivore"))
                        {
                            if(InvenController.Instance.CheckDrug())
                            {
                                Animal anim = hit.collider.GetComponent<Animal>();
                                anim.CureVirusBySyringe();
                                InvenController.Instance.SettingItemNumber(eItemType.Drug, 1, -1);
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator DoingAction()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        Plant plantDetected = mDetectAction.DetectObj.GetComponent<Plant>();

        while(plantDetected == null)
        {
            yield return term;
            plantDetected = mDetectAction.DetectObj.GetComponent<Plant>();
        }
        plantDetected.StartFadeOut();

        Timer effect = IngameManager.Instance.GetEffect(eEffectType.CollectPlant);
        effect.gameObject.SetActive(true);
        effect.transform.position = plantDetected.transform.position;

        mAnim.SetBool("IsCollect", true);

        float max = 100f;
        float current = max;

        plantDetected.SetFadeValue(max, current);

        MainUIController.Instance.OnOffActionGaugeBar(true);
        MainUIController.Instance.ShowActionGaugeBar(max, current);

        while (bPlantAction && (current > 0))
        {
            effect = IngameManager.Instance.GetEffect(eEffectType.CollectPlant);
            effect.gameObject.SetActive(true);
            effect.transform.position = plantDetected.transform.position;

            yield return term;

            current -= 5f;

            plantDetected.SetFadeValue(max, current);

            MainUIController.Instance.ShowActionGaugeBar(max, current);
        }

        if(current <= 0)
        {
            // Get Item from target Obj
            plantDetected.BeingDestroyed();
            InvenController.Instance.SpawnPlantItem(
                plantDetected.gameObject.transform.position,
                plantDetected.gameObject.tag,
                plantDetected.GrowthType
                );

            mStaminaCurrent -= 3;
            
            if(mStaminaCurrent <= 0)
            {
                GameOver();
            }
        }
        else
        {
            plantDetected.IsFadeOut = false;
            plantDetected.OnOffOutline(false);
        }

        mAnim.SetBool("IsCollect", false);

        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.OnOffActionGaugeBar(false);

        if (plantDetected.IsInfect)
        {
            float rand = Random.value;

            if (rand > mImmunity)
            {
                Infect(plantDetected.VirusID);
            }
        }
    }

    private void OpenInvenBox(bool value)
    {
        InvenController.Instance.OpenInvenBox(value);
        bStopMove = value;
    }

    private void OpenUseItemMaker(bool value)
    {
        MakeItemController.Instance.OpenUseItemMaker(value);
        bStopMove = value;
    }

    private void OpenAnalsisTable(bool value)
    {
        AnalysisController.Instance.OnOffAnalysisObj(value);
        bStopMove = value;
    }

    private void OpenDrugMaker(bool value)
    {
        DrugMakerController.Instance.OpenDrugMaker(value);
        bStopMove = value;
    }

    public void Hit(float value)
    {
        mHPcurrent -= value;
        MainUIController.Instance.ShowHPGaugeBar(mHPmax, mHPcurrent);
        if (mHPcurrent <= 0)
        {
            GameOver();
        }
    }

    public void CollectBlood(bool value)
    {
        mCameraYMove.SetCameraPos(value);
        if(mBeaker != null)
        {
            if (mBeaker.IsFullBlood)
            {
                mAnim.SetBool("IsFullBlood", value);
            }
            else
            {
                mAnim.SetBool("IsCollectBlood", value);
            }
        }
    }

    private void OpenEquipItemMaker(bool value)
    {
        MakeItemController.Instance.OpenEquipItemMaker(value);
        bStopMove = value;
    }

    public void BloodInBeaker()
    {
        Blood blood = mDetectAction.DetectObj.GetComponent<Blood>();
        mBeaker.ShowInside(blood.BloodAmount);

        if (mBeakerInfoDic.ContainsKey(blood.VirusID))
            mBeakerInfoDic[blood.VirusID] += blood.BloodAmount;
        else
            mBeakerInfoDic.Add(blood.VirusID, blood.BloodAmount);
    }

    public void UseItem(eUseTarget target, float value)
    {
        switch(target)
        {
            case eUseTarget.Stamina:
                break;
            case eUseTarget.Hunger:

                mHungryCurrent += value;
                if(mHungryCurrent >= mHungryMax)
                {
                    mHungryCurrent = mHungryMax;
                }

                break;
            case eUseTarget.HP:
                break;
            default:
                break;
        }
    }

    public void GetWeaponEquipment(int originalID = -999)
    {
        if (originalID > 0)
        {
            mWeapon = Instantiate(mEquipPrefabs[originalID - 1], mEquipPos[0]).GetComponent<Weapon>();
            mWeapon.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            if (mWeapon != null)
                Destroy(mWeapon.gameObject);
        }
    }

    public void GetBeakerEquipment(int originalID = -999)
    {
        if (originalID > 0)
        {
            mBeaker = Instantiate(mEquipPrefabs[originalID - 1], mEquipPos[1]).GetComponent<Beaker>();
        }
        else
        {
            if (mBeaker != null)
                Destroy(mBeaker.gameObject);
        }
    }

    public void GetSyringeEquipment(int originalID = -999)
    {
        if (originalID > 0)
        {
            mSyringe = Instantiate(mEquipPrefabs[originalID - 1], mEquipPos[1]).GetComponent<Syringe>();
        }
        else
        {
            if (mSyringe != null)
                Destroy(mBeaker.gameObject);
        }
    }

    private void Infect(int virusID)
    {
        mVirusID = virusID;
        bIsInfect = true;
        mStaminaMax *= 0.5f;

        if(mStaminaMax <= mStaminaCurrent)
        {
            mStaminaCurrent = mStaminaMax;
        }

        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
    }

    private void GameOver()
    {
        GameObject go = GameObject.Find("GameOverCanvas");
        GameOverWindow ggWnd = go.GetComponent<GameOverWindow>();
        //ggWnd.OpenGameOverWindow();
        go.SetActive(true);
    }
}
