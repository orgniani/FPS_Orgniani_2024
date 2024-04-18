using System.Collections;
using UnityEngine;

public class MenuInputReader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager levelManager;

    [Header("Screens")]
    [SerializeField] private string animatorParameterClose = "close";

    private void Start()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    public void StartGame()
    {
        levelManager.StartLevel();
    }

    public void OpenScreen(GameObject screen)
    {
        screen.SetActive(true);
    }

    public void CloseScreen(GameObject screen)
    {
        Animator screenAnimator = screen.GetComponent<Animator>();
        screenAnimator.SetTrigger(animatorParameterClose);

        StartCoroutine(PlayAndDeactivate(screen));
    }

    private IEnumerator PlayAndDeactivate(GameObject screen)
    {
        yield return new WaitForSeconds(1f);

        screen.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

