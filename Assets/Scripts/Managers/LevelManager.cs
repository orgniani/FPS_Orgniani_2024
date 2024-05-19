using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Scene indexes")]
    [SerializeField] private int levelBuildIndex = 1;
    [SerializeField] private int mainMenuBuildIndex = 0;

    [Header("Loading")]
    [SerializeField] private int fakeLoadingTime = 2;

    private void Awake()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        Cursor.lockState = CursorLockMode.None;
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
