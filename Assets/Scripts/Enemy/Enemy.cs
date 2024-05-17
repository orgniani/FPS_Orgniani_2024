using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthController HP;

    [SerializeField] private EnemyPatrol patrol;
    [SerializeField] private EnemyArsonist arsonist;

    [SerializeField] private float passedOutDuration = 10f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip wakeUpSound;

    private NavMeshAgent agent;
    private bool isAwake = false;

    public static event Action<Enemy> onSpawn;
    public static event Action<Enemy> onTrapped;
    public static event Action<Enemy> onKnockedOut;
    public static event Action<Enemy> onWakeUp;

    public event Action onWakeUpAnimation;

    private Coroutine wakeUpCoroutine;
    private Coroutine enableCoroutine;

    //public enum ENEMYSTATE { PATROL = 0, FOLLOW_TARGET, STOP }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        onSpawn?.Invoke(this);
    }

    private void OnEnable()
    {
        HP.onHPChange += HandleKnockedOut;
    }

    private void OnDisable()
    {
        HP.onHPChange -= HandleKnockedOut;
    }

    private void HandleKnockedOut()
    {
        if (wakeUpCoroutine != null)
            StopCoroutine(wakeUpCoroutine);
        if (enableCoroutine != null)
            StopCoroutine(enableCoroutine);

        onKnockedOut?.Invoke(this);

        isAwake = false;

        audioSource.PlayOneShot(deathSound);

        if (patrol) patrol.enabled = false;
        if (arsonist) arsonist.enabled = false;

        agent.isStopped = true;

        wakeUpCoroutine = StartCoroutine(WaitToWakeBackUp());
    }

    private IEnumerator WaitToWakeBackUp()
    {
        yield return new WaitForSeconds(passedOutDuration);

        onWakeUp?.Invoke(this);
        onWakeUpAnimation?.Invoke();

        HP.SetToMaxHealth();

        audioSource.PlayOneShot(wakeUpSound);
        isAwake = true;

        enableCoroutine = StartCoroutine(WaitToEnable());
    }

    private IEnumerator WaitToEnable()
    {
        while (!isAwake)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        agent.isStopped = false;

        if (patrol) patrol.enabled = true;
        if (arsonist) arsonist.enabled = true;
    }

    public void HandleGetTrapped()
    {
        onTrapped?.Invoke(this);

        gameObject.SetActive(false);
    }
}
