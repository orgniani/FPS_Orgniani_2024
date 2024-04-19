using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelIndex = 1;
    [SerializeField] private int mainMenuBuildIndex = 0;

    private void Awake()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    public void StartLevel()
    {
        LoadScene(levelIndex);
    }

    public void BackToMenu()
    {
        LoadScene(mainMenuBuildIndex);
    }

    private void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
