using UnityEngine;
using UnityEngine.AI;

public class FollowerEnemy : MonoBehaviour
{
    [SerializeField] private bool playerSpotted = false;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float visionRadius = 5f;
    [SerializeField] private float offset = 0.5f;

    [SerializeField] private Transform target;
    private NavMeshAgent agent;

    [SerializeField] private Transform[] patrolPoints;
    private int currentpatrolPointIndex = 0;

    public VoidDelegate onAttack;

    private enum MovementState { PATROL = 0, FOLLOW }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckIfPlayerSpotted();

        if (!playerSpotted)
        {
            Patrol();
        }

        else
        {
            if(onAttack != null) onAttack();

            agent.SetDestination(target.position);
        }


    }

    void CheckIfPlayerSpotted()
    {
        Vector3 spherePosition = transform.position + transform.forward * offset;
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, visionRadius, playerLayer);

        if (hitColliders.Length > 0)
        {
            if (!playerSpotted)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (target.position - transform.position).normalized, out hit, Mathf.Infinity, ~playerLayer))
                {
                    playerSpotted = false;
                }
                else
                {
                    playerSpotted = true;
                }
            }
        }

        else
        {
            playerSpotted = false;
        }
    }

    void Patrol()
    {
        Vector3 nextPoint = patrolPoints[currentpatrolPointIndex].position;

        float targetDistance = Vector2.Distance(transform.position, nextPoint);

        if (targetDistance < .5f)
        {
            currentpatrolPointIndex++;

            if (currentpatrolPointIndex >= patrolPoints.Length)
            {
                currentpatrolPointIndex = 0;
            }
        }

        agent.SetDestination(nextPoint);

    }

    private void OnDrawGizmos()
    {
        // Draw the sphere in the scene view
        Gizmos.color = playerSpotted ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * offset, visionRadius);
    }

}
