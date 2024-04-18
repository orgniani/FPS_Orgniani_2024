using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HealthController playerHP;
    [SerializeField] private List<HealthController> enemies;
    [SerializeField] private InputReader inputReader;

    [SerializeField] private AudioSource winGameSoundEffect;
    [SerializeField] private AudioSource loseGameSoundEffect;

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
        winGameSoundEffect.Play();
        inputReader.gameObject.SetActive(false);

    }

    private void LoseGame()
    {
        loseGameSoundEffect.Play();
        inputReader.gameObject.SetActive(false);
    }
}
