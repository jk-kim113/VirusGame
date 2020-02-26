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
#pragma warning restore

    private CharacterController mCHControl;

    private bool bPlantAction;
    public bool IsPlantAction { get { return bPlantAction; } set { bPlantAction = value; } }

    private float mStaminaMax;
    private float mStaminaCurrent;

    private float mHungryMax;
    private float mHungryCurrent;

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

        mStaminaMax = 100f;
        mStaminaCurrent = mStaminaMax;

        mHungryMax = 100f;
        mHungryCurrent = mHungryMax;
    }

    private void Start()
    {
        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);
    }

    void Update()
    {
        // Player Move
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir = dir.normalized * mSpeed;
        dir = transform.TransformDirection(dir);
        mCHControl.Move(dir * Time.deltaTime);

        // Player X axis Camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + mouseX, transform.localEulerAngles.z);

        if(Input.GetMouseButtonDown(0))
        {
            if(bPlantAction)
            {   
                StartCoroutine(DoingAction());
            }
        }

        // Player Hungry decrease
        mHungryCurrent -= 0.5f * Time.deltaTime;
        MainUIController.Instance.ShowHungryGaugeBar(mHungryMax, mHungryCurrent);
    }

    private IEnumerator DoingAction()
    {
        WaitForSeconds term = new WaitForSeconds(.1f);

        Plant plantDetected = mDetectAction.DetectObj.GetComponent<Plant>();

        float max = 100f;
        float current = max;

        MainUIController.Instance.OnOffActionGaugeBar(true);
        MainUIController.Instance.ShowActionGaugeBar(max, current);

        while (bPlantAction && current > 0)
        {
            yield return term;

            current -= 5f;

            MainUIController.Instance.ShowActionGaugeBar(max, current);
        }

        if(current <= 0)
        {
            // Get Item from target Obj
            plantDetected.BeingDestroyed();

            mStaminaCurrent -= 3;
            
            if(mStaminaCurrent <= 0)
            {
                GameOver();
            }
        }

        MainUIController.Instance.ShowStaminaGaugeBar(mStaminaMax, mStaminaCurrent);
        MainUIController.Instance.OnOffActionGaugeBar(false);
    }

    private void GameOver()
    {
        // Game Over!!
    }
}
