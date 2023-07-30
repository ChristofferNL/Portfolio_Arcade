using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
/// <summary>
/// Main class for the Player object
/// </summary>
public class Player : AnimatedObject, IDamageable
{
    public PlayerAttributes PlayerAttributes;
    public PlayerAttack PlayerAttack;
    public PlayerCollision PlayerCollision;
    public PlayerMove PlayerMove;
    public override Animator Animator { get; set; }
    public override SpriteRenderer SpriteRenderer { get; set; }
    public int Hitpoints { get; set; }
    public override ObjectStates ActiveState { get; set; } = new();
    public override Dictionary<ObjectStates, string> AnimationValuePairs { get; set; } = new Dictionary<ObjectStates, string>() 
    {
        { ObjectStates.Idle, "Player_Idle"},
        { ObjectStates.InAir, "Player_Jump"}, 
        { ObjectStates.Running, "Player_Run"},
        { ObjectStates.Walking, "Player_Walk"},
        { ObjectStates.CanWalljump, "Player_Walljump"},
        { ObjectStates.Shooting, "Player_Shoot_Standing"},
        { ObjectStates.RunningShooting, "Player_Shoot_Run"},
        { ObjectStates.WalkingShooting, "Player_Shoot_Walk"},
        { ObjectStates.TakingDamage, "Player_Taking_Damage"}
    };
    private void Awake()
    {
        PlayerAttributes.PlayerVelocity = 0;
        PlayerAttributes.GravitationalForce = 0;
        Hitpoints = PlayerAttributes.HitPoints;
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerCollision = GetComponent<PlayerCollision>();
        PlayerMove = GetComponent<PlayerMove>();
        Animator = GetComponentInChildren<Animator>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
    }
    private void Update()
    {
        if (PlayerAttributes.MovingLeft)
        {
            SpriteRenderer.flipX = true;
        }
        else if (PlayerAttributes.MovingRight)
        {
            SpriteRenderer.flipX = false;
        }
        SetActiveState();
        CheckAnimationState();
        PlayerMove.ApplyGravity();
        PlayerMove.XAxisMoveCalc();
        PlayerMove.YAxisMoveCalc();
        PlayerMove.WalljumpTimer();
        PlayerMove.CalculateInAirXVelocity();
        PlayerMove.ExecuteMove(PlayerAttributes.GravitationalForce, PlayerAttributes.MoveDirection);
        PlayerAttack.ShootCooldownTimer();
        DamageCooldownTimer();
    }
    /// <summary>
    /// Determines what ObjectStates ActiveState the Player is currently in.
    /// </summary>
    private void SetActiveState()
    {
        if (!PlayerAttributes.CanTakeDamage)
        {
            ActiveState = ObjectStates.TakingDamage;
        }
        else if (PlayerAttributes.Grounded && !PlayerAttributes.Walking && !PlayerAttributes.Running && !PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.Idle;
        }
        else if (!PlayerAttributes.Grounded && !PlayerAttributes.CanWalljump && !PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.InAir;
        }
        else if (!PlayerAttributes.Grounded && PlayerAttributes.IsShooting || !PlayerAttributes.Running && !PlayerAttributes.Walking && PlayerAttributes.IsShooting && PlayerAttributes.Grounded)
        {
            ActiveState = ObjectStates.Shooting;
        }
        else if (PlayerAttributes.Walking && !PlayerAttributes.Running && PlayerAttributes.IsShooting && PlayerAttributes.Grounded)
        {
            ActiveState = ObjectStates.WalkingShooting;
        }
        else if (PlayerAttributes.Running && PlayerAttributes.IsShooting && PlayerAttributes.Grounded)
        {
            ActiveState = ObjectStates.RunningShooting;
        }
        else if (!PlayerAttributes.Grounded && !PlayerAttributes.IsShooting && PlayerAttributes.CanWalljump)
        {
            ActiveState = ObjectStates.CanWalljump;
        }
        else if (!PlayerAttributes.Grounded && PlayerAttributes.IsShooting && !PlayerAttributes.CanWalljump)
        {
            ActiveState = ObjectStates.InAirShooting;
        }
        else if (PlayerAttributes.Grounded && PlayerAttributes.Walking && !PlayerAttributes.Running && !PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.Walking;
        }
        else if (PlayerAttributes.Grounded && PlayerAttributes.Walking && !PlayerAttributes.Running && PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.WalkingShooting;
        }
        else if (PlayerAttributes.Grounded && PlayerAttributes.Walking && PlayerAttributes.Running && !PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.Running;
        }
        else if (PlayerAttributes.Grounded && PlayerAttributes.Walking && PlayerAttributes.Running && PlayerAttributes.IsShooting)
        {
            ActiveState = ObjectStates.RunningShooting;
        }
    }
    public void TakeDamage(int damage)
    {
        Hitpoints -= damage;
        PlayerAttributes.CanTakeDamage = false;
        PlayerAttributes.DamageCooldown = 0;
        EventManagerGlobal.Instance.TakeDamage(Hitpoints);
        if (Hitpoints <= 0)
        {
            Death();
        }
    }
    /// <summary>
    /// Runs after a player has taken damage until the DamageCooldown is equal to DamageCooldownSeconds
    /// </summary>
    private void DamageCooldownTimer()
    {
        if (PlayerAttributes.DamageCooldown < PlayerAttributes.DamageCooldownSeconds)
        {
            PlayerAttributes.DamageCooldown += Time.deltaTime;
            if (PlayerAttributes.DamageCooldown >= PlayerAttributes.DamageCooldownSeconds)
            {
                PlayerAttributes.DamageCooldown = PlayerAttributes.DamageCooldownSeconds;
                PlayerAttributes.CanTakeDamage = true;
            }
        }
        else
        {
            PlayerAttributes.DamageCooldown = PlayerAttributes.DamageCooldownSeconds;
        }
    }
    public void Death()
    {
        SceneManager.LoadScene("StartingScreenScene");
    }
}
