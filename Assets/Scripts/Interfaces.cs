using UnityEngine;

public interface IAttack
{
    float AttackNow(Transform target, HealthController targetHP);
}

public interface IHittable
{
    public void ReceiveDamage(float damage);
}
