using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HealthController playerHP;
    [SerializeField] private List<HealthController> enemies;
    [SerializeField] private InputReader inputReader;

    [SerializeField] private AudioSource winGameSoundEffect;
    [SerializeField] private AudioSource loseGameSoundEffect;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelCompletedScreen;

    [SerializeField] private UITimeCounter timeCounter;

    private void OnEnable()
    {

        playerHP.onDead += LoseGame;

        for(int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].onDead += KillCounter;
        }
    }

    private void OnDisable()
    {
        playerHP.onDead -= LoseGame;
    }

    private void KillCounter()
    {
        for(int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].getHealth() <= 0)
            {
                enemies[i].onDead -= KillCounter;
                enemies.Remove(enemies[i]);
            }

            if (enemies.Count == 0)
            {
                WinGame();
            }
        }
    }

    private void WinGame()
    {
        StopGameAndOpenScreens(levelCompletedScreen, winGameSoundEffect);
    }

    private void LoseGame()
    {
        StopGameAndOpenScreens(gameOverScreen, loseGameSoundEffect);
    }

    private void StopGameAndOpenScreens(GameObject screen, AudioSource soundEffect)
    {
        Cursor.lockState = CursorLockMode.None;

        soundEffect.Play();
        screen.SetActive(true);

        foreach(HealthController enemy in enemies)
        {
            if (enemy.TryGetComponent(out Enemy enemyScript))
                enemyScript.enabled = false;

            //enemy.gameObject.SetActive(false);
        }

        inputReader.gameObject.SetActive(false);
        timeCounter.StopCounting();
    }
}
