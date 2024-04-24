using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController playerHP;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private InputReader inputReader;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource winGameSoundEffect;
    [SerializeField] private AudioSource loseGameSoundEffect;

    [Header("Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelCompletedScreen;

    [Header("Timer")]
    [SerializeField] private UITimeCounter timeCounter;

    private void OnEnable()
    {
        playerHP.onDead += LoseGame;
        Enemy.onSpawn += SpawnedEnemy;
        Enemy.onDeath += KillCounter;
    }

    private void OnDisable()
    {
        playerHP.onDead -= LoseGame;
        Enemy.onSpawn -= SpawnedEnemy;
        Enemy.onDeath -= KillCounter;
    }

    private void KillCounter(Enemy obj)
    {
        enemies.Remove(obj);

        if (enemies.Count == 0)
        {
            StopGameAndOpenScreens(levelCompletedScreen, winGameSoundEffect);
        }
    }

    private void SpawnedEnemy(Enemy obj)
    {
        enemies.Add(obj);
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

        foreach(Enemy enemy in enemies)
        {
            enemy.enabled = false;
        }

        inputReader.gameObject.SetActive(false);
        timeCounter.StopCounting();
    }
}
