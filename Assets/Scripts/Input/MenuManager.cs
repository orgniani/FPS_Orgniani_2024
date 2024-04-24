using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Slider loadBar;

    [Header("Screen Animation Parameters")]
    [SerializeField] private string animatorParameterClose = "close";
    [SerializeField] private float screenAnimationDuration = 1.5f;

    private void Update()
    {
        if (!loadBar) return;
        loadBar.value = (LoaderManager.Get().loadingProgress);
    }

    public void StartGame(GameObject screen)
    {
        loadBar.value = 0;
        OpenScreen(screen);
        levelManager.StartLevel();
    }

    public void GoBackToMenu(GameObject screen)
    {
        PauseAndUnpauseGame();

        loadBar.value = 0;
        OpenScreen(screen);
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
        yield return new WaitForSeconds(screenAnimationDuration);

        screen.SetActive(false);
    }

    private IEnumerator PlayAndPauseGame()
    {
        yield return new WaitForSeconds(screenAnimationDuration);

        Time.timeScale = 0;
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();
    }
}

