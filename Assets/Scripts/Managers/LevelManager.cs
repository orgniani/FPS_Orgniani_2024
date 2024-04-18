using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelIndex = 1;
    [SerializeField] private int mainMenuBuildIndex = 0;

    public void StartLevel()
    {
        LoadScene(levelIndex);
    }

    public void BackToMenu()
    {
        LoadScene(mainMenuBuildIndex);
    }

    public void LoadScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
