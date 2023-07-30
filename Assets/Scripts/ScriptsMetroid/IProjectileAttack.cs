using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Interface used by objects that will be able to perform projectile attacks
/// </summary>
public interface IProjectileAttack
{
    public abstract float ShootCooldown { get; set; }
    public abstract float ShootCooldownSeconds { get; set; }
    public abstract void ProcessShoot(bool shoot);
    public abstract void ShootCooldownTimer();
}
