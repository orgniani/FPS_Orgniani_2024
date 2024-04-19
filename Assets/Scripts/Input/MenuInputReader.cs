using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class MenuInputReader : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private string animatorParameterClose = "close";

    public void StartGame()
    {
        levelManager.StartLevel();
    }

    public void GoBackToMenu()
    {
        levelManager.BackToMenu();
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

    public void PauseAndUnpauseGame()
    {
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;

            Animator screenAnimator = pauseScreen.GetComponent<Animator>();
            screenAnimator.SetTrigger(animatorParameterClose);

            StartCoroutine(PlayAndDeactivate(pauseScreen));
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            pauseScreen.SetActive(true);

            StartCoroutine(PlayAndPauseGame());
        }
    }

    private IEnumerator PlayAndDeactivate(GameObject screen)
    {
        yield return new WaitForSeconds(1.5f);

        screen.SetActive(false);
    }

    private IEnumerator PlayAndPauseGame()
    {
        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0;
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();
    }
}

