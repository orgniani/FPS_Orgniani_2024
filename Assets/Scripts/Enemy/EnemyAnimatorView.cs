using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private MeleeAttack attack;
    [SerializeField] private EnemyArsonist arsonist;
    [SerializeField] private Enemy enemy;
    [SerializeField] private HealthController HP;
    [SerializeField] private NavMeshAgent agent;

    [Header("Animator Parameters")]
    [SerializeField] private string speedParameter = "speed";
    [SerializeField] private string punchTriggerParameter = "punch";
    [SerializeField] private string hurtTriggerParameter = "get_hit";
    [SerializeField] private string lightFireTriggerParameter = "light_fire";
    [SerializeField] private string dieTriggerParameter = "die";
    [SerializeField] private string wakeUpTriggerParameter = "wake_up";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        attack.onPunch += HandlePunch;
        HP.onHPChange += HandleHurt;
        HP.onDead += HandleDeath;
        enemy.onWakeUpAnimation += HandleWakeUp;

        if (arsonist) arsonist.onLightFire += HandleLightFire;
    }

    private void OnDisable()
    {
        attack.onPunch -= HandlePunch;
        HP.onHPChange -= HandleHurt;
        HP.onDead -= HandleDeath;
        enemy.onWakeUpAnimation -= HandleWakeUp;

        if (arsonist) arsonist.onLightFire -= HandleLightFire;
    }

    private void Update()
    {
        var speed = agent.velocity.magnitude;

        if (animator)
            animator.SetFloat(speedParameter, speed);
    }

    private void HandlePunch()
    {
        animator.SetTrigger(punchTriggerParameter);
    }

    private void HandleHurt()
    {
        animator.SetTrigger(hurtTriggerParameter);
    }

    private void HandleDeath()
    {
        animator.SetTrigger(dieTriggerParameter);
    }

    private void HandleWakeUp()
    {
        animator.SetTrigger(wakeUpTriggerParameter);
    }

    private void HandleLightFire()
    {
        animator.SetTrigger(lightFireTriggerParameter);
    }
}
