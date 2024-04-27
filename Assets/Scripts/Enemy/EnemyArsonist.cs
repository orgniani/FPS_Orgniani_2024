using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArsonist : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeleeAttack attack;
    [SerializeField] private HealthController HP;

    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Transform target;

    [Header("Parameters")]
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private float maxDistanceToTarget = 5f;

    [SerializeField] private float patrolSpeed = 4f;
    [SerializeField] private float chaseSpeed = 8f;

    private bool playerSpotted = false;
    private bool shouldLightFire = true;

    private NavMeshAgent agent;
    private HealthController playerHP;

    private int currentPatrolPointIndex = 0;

    public event Action onLightFire = delegate { };

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHP = target.gameObject.GetComponent<HealthController>();
    }

    private void OnEnable()
    {
        HP.onHurt += SpotPlayer;

        FlammableObject.onSpawn += LightOnFireTargets;
        FlammableObject.onExtinguished += LightOnFireTargets;
        FlammableObject.onFire += RemoveFromLightOnFire;
        FlammableObject.onDeath += RemoveFromLightOnFire;
    }

    private void OnDisable()
    {
        HP.onHurt -= SpotPlayer;

        foreach(Transform point in patrolPoints)
        {
            FlammableObject.onSpawn -= LightOnFireTargets;
            FlammableObject.onExtinguished -= LightOnFireTargets;
            FlammableObject.onFire -= RemoveFromLightOnFire;
            FlammableObject.onDeath -= RemoveFromLightOnFire;
        }
    }

    private void RemoveFromLightOnFire(FlammableObject obj)
    {
        patrolPoints.Remove(obj.transform);
    }

    private void LightOnFireTargets(FlammableObject obj)
    {
        if (obj == null) return;
        if (!obj.gameObject.activeSelf) return;
        patrolPoints.Add(obj.transform);
    }

    private void Update()
    {
        if (!playerSpotted)
        {
            Patrol();
            agent.speed = patrolSpeed;
        }

        else
        {
            if (playerHP.Health <= 0) return;

            //float waitTime = attack.AttackNow();
            attack.AttackNow(target, playerHP);
            agent.SetDestination(target.position);

            agent.speed = chaseSpeed;
        }
    }

    private void SpotPlayer()
    {
        playerSpotted = true;
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(chaseDuration);

        agent.isStopped = false;
        playerSpotted = false;
    }


    private void Patrol()
    {
        if (!shouldLightFire) return;

        if (patrolPoints.Count == 0)
        {
            agent.isStopped = true;
            return;
        }
        else agent.isStopped = false;

        Vector3 nextPoint = patrolPoints[currentPatrolPointIndex].transform.position;
        float distanceToCurrentTarget = Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position);

        //Debug.Log(currentPatrolPointIndex);
        //Debug.Log(distanceToCurrentTarget);

        if (distanceToCurrentTarget <= maxDistanceToTarget)
        {
            StartCoroutine(StopAndLight());

            FlammableObject flammableObject = patrolPoints[currentPatrolPointIndex].GetComponent<FlammableObject>();
            flammableObject.HandleGetLitOnFire();

            SetNextPatrolPoint(distanceToCurrentTarget);
        }

        agent.SetDestination(nextPoint);
    }

    private IEnumerator StopAndLight()
    {
        shouldLightFire = false;

        agent.isStopped = true;
        onLightFire?.Invoke();

        yield return new WaitForSeconds(1f);

        shouldLightFire = true;
        agent.isStopped = false;
    }

    private void SetNextPatrolPoint(float distanceToCurrentTarget)
    {
        //agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        //currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;

        if (distanceToCurrentTarget < maxDistanceToTarget)
        {
            currentPatrolPointIndex++;

            if (currentPatrolPointIndex >= patrolPoints.Count)
            {
                currentPatrolPointIndex = 0;
            }
        }
    }
}
