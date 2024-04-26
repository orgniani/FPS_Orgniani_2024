using UnityEngine;

public interface IAttack
{
    public float AttackNow(Transform target, HealthController targetHP);
}
