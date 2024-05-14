using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyArsonist : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Transform target;

    private AudioSource audioSource;
    [SerializeField] private AudioClip lightOnFireSound;

    [Header("Parameters")]
    [SerializeField] private float maxDistanceToTarget = 5f;
    [SerializeField] private float patrolSpeed = 4f;

    private bool shouldLightFire = true;

    private NavMeshAgent agent;

    private int currentPatrolPointIndex = 0;

    public event Action onLightFire = delegate { };

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        FlammableObject.onSpawn += LightOnFireTargets;
        FlammableObject.onExtinguished += LightOnFireTargets;
        FlammableObject.onFire += RemoveFromLightOnFire;
        FlammableObject.onDeath += RemoveFromLightOnFire;
    }

    private void OnDisable()
    {
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
        Patrol();
        agent.speed = patrolSpeed;
    }

    private void Patrol()
    {
        if (!shouldLightFire) return;

        if (patrolPoints.Count == 0)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

        Vector3 nextPoint = patrolPoints[currentPatrolPointIndex].transform.position;
        float distanceToCurrentTarget = Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position);

        if (distanceToCurrentTarget <= maxDistanceToTarget)
        {
            StartCoroutine(StopAndLight());

            FlammableObject flammableObject = patrolPoints[currentPatrolPointIndex].GetComponent<FlammableObject>();
            flammableObject.HandleGetLitOnFire();

            if (lightOnFireSound) audioSource.PlayOneShot(lightOnFireSound);

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
