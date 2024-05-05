using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController HP;

    [SerializeField] private EnemyPatrol patrol;
    [SerializeField] private EnemyArsonist arsonist;

    [SerializeField] private AudioSource deathSound;

    private NavMeshAgent agent;

    public static event Action<Enemy> onSpawn;
    public static event Action<Enemy> onDeath;


    public enum ENEMYSTATE { PATROL = 0, FOLLOW_TARGET, STOP }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        onSpawn?.Invoke(this);
    }

    private void OnEnable()
    {
        HP.onHurt += HandleHurt;
        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onHurt -= HandleHurt;
        HP.onDead -= HandleDeath;
    }

    private void HandleDeath()
    {
        onDeath?.Invoke(this);

        agent.isStopped = true;
        enabled = false;

        deathSound.Play();

        if (patrol) patrol.enabled = false;
        if (arsonist) arsonist.enabled = false;
    }

    private void HandleHurt()
    {
        if (HP.Health <= 0) return;

        StartCoroutine(StopMovingAfterHit());
    }

    private IEnumerator StopMovingAfterHit()
    {
        agent.isStopped = true;
        enabled = false;

        yield return new WaitForSeconds(0.5f);

        agent.isStopped = false;
        enabled = true;
    }

}
