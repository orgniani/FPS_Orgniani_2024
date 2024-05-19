using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController playerHP;
    [SerializeField] private InputReader inputReader;

    [Header("Lists")]
    [SerializeField] private List<Enemy> enemies;
    public List<FlammableObject> flammables;

    [Header("Audio")]
    [SerializeField] private AudioSource winGameSoundEffect;
    [SerializeField] private AudioSource loseGameSoundEffect;

    [Header("Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject levelCompletedScreen;

    [Header("Text")]
    [SerializeField] private UITimeCounter timeCounter;
    [SerializeField] private TextMeshProUGUI gameOverText;

    public int flammablesTotal;

    public event Action onNewDeadTree = delegate { };

    private void OnEnable()
    {
        playerHP.onDead += LoseGame;

        Enemy.onSpawn += SpawnedEnemy;
        Enemy.onTrapped += KillCounter;

        FlammableObject.onSpawn += SpawnedFlammable;
        FlammableObject.onDeath += DeadNatureCounter;
    }

    private void OnDisable()
    {
        playerHP.onDead -= LoseGame;

        Enemy.onSpawn -= SpawnedEnemy;
        Enemy.onTrapped -= KillCounter;

        FlammableObject.onSpawn -= SpawnedFlammable;
        FlammableObject.onDeath -= DeadNatureCounter;
    }

    private void KillCounter(Enemy obj)
    {
        enemies.Remove(obj);

        if (enemies.Count == 0)
        {
            StopGameAndOpenScreens(levelCompletedScreen, winGameSoundEffect);
        }
    }

    private void DeadNatureCounter(FlammableObject obj)
    {
        flammables.Remove(obj);
        onNewDeadTree?.Invoke();

        if (flammables.Count == 0)
        {
            LoseGame();
            gameOverText.text = "THE FOREST WAS DESTROYED!";
        }
    }

    private void SpawnedEnemy(Enemy obj)
    {
        enemies.Add(obj);
    }

    private void SpawnedFlammable(FlammableObject obj)
    {
        flammables.Add(obj);
        flammablesTotal++;
    }

    private void LoseGame()
    {
        StopGameAndOpenScreens(gameOverScreen, loseGameSoundEffect);
        gameOverText.text = "YOU DIED!";
    }

    private void StopGameAndOpenScreens(GameObject screen, AudioSource soundEffect)
    {
        Cursor.lockState = CursorLockMode.None;

        soundEffect.Play();
        screen.SetActive(true);

        inputReader.gameObject.SetActive(false);
        timeCounter.StopCounting();

        enabled = false;
    }
}
