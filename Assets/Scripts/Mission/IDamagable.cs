using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    float MaxHealth { get; }
    float CurrentHealth { get; }


    void ReciveDamage(float damage);
    void ReciveDamage(float damage, Vector3 hitPoint, Vector3 hitDirection);

    void ReciveDamageEffect();

}
