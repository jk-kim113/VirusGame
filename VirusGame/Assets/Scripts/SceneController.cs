using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

#pragma warning disable 0649
    [SerializeField]
    private GameObject mLoadingWnd;
#pragma warning restore

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

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartLoadIngameScene();
    }

    public void StartLoadIngameScene()
    {   
        StartCoroutine(LoadingProcess("Ingame"));
    }

    private IEnumerator LoadingProcess(string sceneName)
    {   
        GameObject go = Instantiate(mLoadingWnd, transform);
        LoadingWindow wnd = go.GetComponent<LoadingWindow>();
        wnd.OpenLoadingWnd();

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
        {
            wnd.ShowGaugeBar(asyncOp.progress);
            yield return null;
        }

        wnd.ShowGaugeBar(1);
        yield return new WaitForSeconds(1.8f);

        Destroy(wnd.gameObject);
        yield return new WaitForSeconds(1f);
    }
}
