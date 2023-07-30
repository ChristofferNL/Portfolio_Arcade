using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ScriptableObject that contains all variables needed for classes connected to the Player object
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    [Header("X-Axis Velocity")]
    public float PlayerVelocity;
    public float CounterWalktime;
    public float CounterRuntime;
    public float MoveDirection;
    [Header("Y-Axis Velocity")]
    public float GravitationalForce;
    public float CounterJumptime;
    public float CounterJumptimeSeconds;
    public float JumpVelocity;
    public float DirectionChangedMaxAirSpeed;
    public float JumpStartingVelocity;
    public float WalljumpCooldown;
    public float WalljumpCooldownSeconds;
    public int NumberOfJumps;
    public int WalljumpsLeft;
    [Header("Movement")]
    public float JumpSpeed;    
    public float MaxspeedWalk;
    public float MaxspeedRun;
    public float MaxspeedGravity;
    public float MaxspeedJump;
    public float RunspeedMultiplier;
    public float MovespeedMultiplier;
    public int MoveDivider;
    [Header("Gravity")] 
    public float VerticalForceMultiplier;
    [Header("Bools")]
    public bool Grounded;
    public bool Jumping;
    public bool SpinJumping;
    public bool Walljumping;
    public bool Walking;
    public bool Running;
    public bool MovingRight;
    public bool MovingLeft;
    public bool CanJump;
    public bool CanWalljump;
    public bool CollidingUp;
    public bool CollidingLeft;
    public bool CollidingRight;
    public bool FacingRight;
    public bool IsShooting;
    public bool CanTakeDamage;
    public bool CanActivateTeleporter;
    [Header("Stats")]
    public int HitPoints;
    public float DamageCooldown;
    public float DamageCooldownSeconds;
}
