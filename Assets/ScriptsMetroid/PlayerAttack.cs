using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Contains methods for the Player to shoot and calculate the cooldown
/// </summary>
public class PlayerAttack : MonoBehaviour, IProjectileAttack
{
    public Player Player;
    public Bullet Bullet;
    public float ShootCooldown { get; set; }
    public float ShootCooldownSeconds { get; set; }

    private void Awake()
    {
        ShootCooldown = 0.35f;
        ShootCooldownSeconds = 0.35f;
        Player = GetComponent<Player>();
    }
    /// <summary>
    /// Is called from the InputManager class, instantiates a clone of the bullet prefab reference connected, sets the movedirection of the bullet and activates the timer for ShootCooldown
    /// </summary>
    /// <param name="shoot"></param>
    public void ProcessShoot(bool shoot)
    {
        if (shoot && ShootCooldown == ShootCooldownSeconds && Player.PlayerAttributes.FacingRight)
        {
            Bullet clone = Instantiate(Bullet, new Vector3(transform.position.x + 1, transform.position.y + 0.4f, transform.position.z), Quaternion.identity);
            ShootCooldown = 0;
            Player.PlayerAttributes.IsShooting = true;
            clone.MovingRight = true;
            EventManagerGlobal.Instance.SoundShoot();
        }
        if (shoot && ShootCooldown == ShootCooldownSeconds && !Player.PlayerAttributes.FacingRight)
        {
            Bullet clone = Instantiate(Bullet, new Vector3(transform.position.x - 1, transform.position.y + 0.4f, transform.position.z), Quaternion.identity);
            ShootCooldown = 0;
            Player.PlayerAttributes.IsShooting = true;
            clone.MovingRight = false;
            EventManagerGlobal.Instance.SoundShoot();
        }
    }
    /// <summary>
    /// Runs a cooldown timer based on the ShootCooldownSeconds variable and resets the isShooting bool
    /// </summary>
    public void ShootCooldownTimer()
    {
        if (ShootCooldown <= ShootCooldownSeconds)
        {
            ShootCooldown += Time.deltaTime;
        }
        else
        {
            Player.PlayerAttributes.IsShooting = false;
            ShootCooldown = ShootCooldownSeconds;
        }
    }

}
