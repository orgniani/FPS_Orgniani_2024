using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelBuildIndex = 1;
    [SerializeField] private int mainMenuBuildIndex = 0;

    [SerializeField] private int fakeLoadingTime = 2;

    private void Awake()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    public void StartLevel()
    {
        LoadAndOpen(levelBuildIndex);
    }

    public void BackToMenu()
    {
        LoadAndOpen(mainMenuBuildIndex);
    }

    private void LoadAndOpen(int sceneBuildIndex)
    {
        LoaderManager.Get().LoadScene(sceneBuildIndex, fakeLoadingTime);
    }
}
