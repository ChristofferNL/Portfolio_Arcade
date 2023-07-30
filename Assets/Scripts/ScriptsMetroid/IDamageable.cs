using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Interface used by all objects that can take damage
/// </summary>
public interface IDamageable
{
    public int Hitpoints { get; set; }
    public void TakeDamage(int damage);
    public void Death();
}
