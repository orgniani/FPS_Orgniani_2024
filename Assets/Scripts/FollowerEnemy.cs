using UnityEngine;
using UnityEngine.AI;

public class FollowerEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }

}
