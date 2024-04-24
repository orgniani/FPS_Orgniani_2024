public interface IAttack
{
    float AttackNow();
}

public interface IHittable
{
    public void ReceiveDamage(float damage);
}
