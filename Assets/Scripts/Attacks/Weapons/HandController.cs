using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HandController : MonoBehaviour
{
    [SerializeField] private FirstPersonController FPSController;
    [SerializeField] private float proximityRadius = 5f;
    [SerializeField] private float dragSpeed = 2f;

    public List<Transform> knockedOutEnemies;
    private NavMeshAgent enemyAgent;

    private Transform currentlyDraggedEnemy;

    private bool drag = false;
    private bool isDraggingEnemy = false;

    public bool IsDraggingEnemy => isDraggingEnemy;

    public bool CanDrag { set; get; }

    public bool IsAtTheDoor { set; get; }

    private void OnEnable()
    {
        Enemy.onKnockedOut += AddToEnemiesList;
        Enemy.onWakeUp += RemoveFromEnemiesList;
        Enemy.onTrapped += RemoveFromEnemiesList;
    }

    private void OnDisable()
    {
        Enemy.onKnockedOut -= AddToEnemiesList;
        Enemy.onWakeUp -= RemoveFromEnemiesList;
        Enemy.onTrapped -= RemoveFromEnemiesList;
    }

    private void Update()
    {
        if (drag)
        {
            DragEnemy();
        }

        else
        {
            StopDragging();
        }
    }

    public void Drag(bool isDragging)
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!enabled) return;

        //currentlyDraggedEnemy = null;

        drag = isDragging;
    }

    private void DragEnemy()
    {
        if (!CanDrag) return;

        Transform nearestEnemy = FindNearestKnockedOutEnemy();

        if (nearestEnemy != null)
        {
            float distanceToNearestEnemy = Vector3.Distance(transform.position, nearestEnemy.position);

            if (distanceToNearestEnemy <= proximityRadius)
            {
                enemyAgent = nearestEnemy.GetComponent<NavMeshAgent>();

                if (enemyAgent != null)
                {
                    if (enemyAgent == null) return;

                    enemyAgent.isStopped = false;
                    enemyAgent.SetDestination(transform.position);

                    FPSController.CanSprint = false;
                    FPSController.Speed = dragSpeed;

                    isDraggingEnemy = true;
                    currentlyDraggedEnemy = nearestEnemy;
                }
            }

            else
            {
                if (enemyAgent == null) return;
                enemyAgent.isStopped = true;
            }
        }

        else
        {
            StopDragging();

            if (enemyAgent == null) return;
            enemyAgent.isStopped = false;
        }
    }

    private void StopDragging()
    {
        isDraggingEnemy = false;
        FPSController.CanSprint = true;
        FPSController.Speed = 4f;

        if (currentlyDraggedEnemy != null) currentlyDraggedEnemy = null;

    }

    private Transform FindNearestKnockedOutEnemy()
    {
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemyTransform in knockedOutEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);

            if (distanceToEnemy < minDistance)
            {
                nearestEnemy = enemyTransform;
                minDistance = distanceToEnemy;
            }
        }

        return nearestEnemy;
    }

    private void AddToEnemiesList(Enemy obj)
    {
        knockedOutEnemies.Add(obj.transform);
    }

    private void RemoveFromEnemiesList(Enemy obj)
    {
        knockedOutEnemies.Remove(obj.transform);
    }

    public void HandleTrapGoblin()
    {
        if (!IsAtTheDoor || currentlyDraggedEnemy == null) return;

        Enemy enemyScript = currentlyDraggedEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.HandleGetTrapped();
        }

        enemyAgent = null;
    }
}
