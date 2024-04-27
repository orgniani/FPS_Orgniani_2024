using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    private static LoaderManager instance;

    public float loadingProgress;
    private float timeLoading;
    private float fakeLoadTime = 2;

    public static event Action<LoaderManager> OnLoadingStart;
    public static event Action<LoaderManager> OnLoadingEnd;

    public static LoaderManager Get()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(int sceneIndex, float fakeTime = -1)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        fakeTime = fakeTime < 0.01f ? fakeLoadTime : fakeTime;

        StartCoroutine(AsynchronousLoadWithFake(sceneIndex, fakeTime));

    }

    IEnumerator AsynchronousLoadWithFake(int sceneIndex, float fakeTime)
    {
        OnLoadingStart?.Invoke(this);

        loadingProgress = 0;
        timeLoading = 0;
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            timeLoading += Time.unscaledDeltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / fakeTime;

            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }

        OnLoadingEnd?.Invoke(this);
    }
}
