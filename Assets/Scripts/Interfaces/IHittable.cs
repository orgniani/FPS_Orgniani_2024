using UnityEngine;

public interface IHittable
{
    public void ReceiveDamage(float damage, Vector3 hitPoint);
}
