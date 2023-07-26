using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base abstract class for every type of enemy
/// </summary>
public abstract class Enemy : AnimatedObject, IDamageable
{
    public abstract int Hitpoints { get; set; }
    public abstract int DamageDealt { get; set; }
    public abstract bool Destroyed { get; set; }
    public abstract void Death();
    public virtual void TakeDamage(int damage)
    {
        Hitpoints -= damage;
        if (Hitpoints <= 0)
        {
            Death();
        }
    }
}
