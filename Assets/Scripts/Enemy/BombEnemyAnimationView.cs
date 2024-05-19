using UnityEngine;
using UnityEngine.AI;

public class BombEnemyAnimationView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private BombEnemy bomb;

    [Header("Animator Parameters")]
    [SerializeField] private string speedParameter = "speed";
    [SerializeField] private string closeToPlayerTriggerParameter = "close_to_player";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        bomb.onCloseToPlayer += HandleLookAround;
    }

    private void OnDisable()
    {
        bomb.onCloseToPlayer -= HandleLookAround;
    }

    private void Update()
    {
        var speed = agent.velocity.magnitude;

        if (animator)
            animator.SetFloat(speedParameter, speed);
    }

    private void HandleLookAround()
    {
        animator.SetTrigger(closeToPlayerTriggerParameter);
    }
}
