using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    public enum eGameState
    {
        Intro       = 0,
        NewGame,
        LoadGame,
        PlayGame,
        EndGame
    }

    public static IngameManager Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mIntroUICanvas;
    [SerializeField]
    private Camera mIntroCamera;
    [SerializeField]
    private GameObject mFadeOutCanvas;
    [SerializeField]
    private GameObject mIngameUICanvas;
    [SerializeField]
    private EffectPool mEffectPool;
#pragma warning restore

    private eGameState mCurrentGameState;
    public eGameState CurrentGameState { get { return mCurrentGameState; } }

    private Image mFadeOutImg;
    private GameObject mIntroUICanvasObj;

    private float mPlayTime;

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
    }

    private void Start()
    {
        mFadeOutImg = mFadeOutCanvas.GetComponentInChildren<Image>();

        IntroGame();
    }

    private void Update()
    {
        if(mCurrentGameState == eGameState.NewGame)
        {
            mFadeOutImg.color = new Color(mFadeOutImg.color.r, mFadeOutImg.color.g, mFadeOutImg.color.b, mFadeOutImg.color.a + .4f * Time.deltaTime);
            if(mFadeOutImg.color.a >= 0.9f)
            {
                PlayGame();
            }
        }

        if(mCurrentGameState == eGameState.PlayGame)
        {
            mPlayTime += Time.deltaTime;
            MainUIController.Instance.ShowPlayTime(mPlayTime);
        }
    }

    public void IntroGame()
    {
        mCurrentGameState = eGameState.Intro;

        mIntroUICanvasObj = Instantiate(mIntroUICanvas);
        mIntroUICanvasObj.SetActive(true);
        mIntroCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Icon"));
    }

    public void StartNewGame()
    {
        mCurrentGameState = eGameState.NewGame;
        
        mFadeOutImg.color = new Color(mFadeOutImg.color.r, mFadeOutImg.color.g, mFadeOutImg.color.b, 0);
        mIngameUICanvas.gameObject.SetActive(false);
        
        mFadeOutCanvas.SetActive(true);
    }

    public void PlayGame()
    {
        mCurrentGameState = eGameState.PlayGame;

        mIntroUICanvasObj.SetActive(false);
        mIntroCamera.gameObject.SetActive(false);
        mFadeOutCanvas.gameObject.SetActive(false);
        mIngameUICanvas.gameObject.SetActive(true);
    }

    public Timer GetEffect(eEffectType type)
    {
        return mEffectPool.GetFromPool((int)type);
    }
}
