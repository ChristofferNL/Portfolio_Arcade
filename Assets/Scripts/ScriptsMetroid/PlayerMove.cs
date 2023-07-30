using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Contains all methods for calculating and executing the movement of the player, movement is done via Mathf.Lerp in Player.Update()
/// </summary>
public class PlayerMove : MonoBehaviour, IMoveable
{
    public Player Player;
    public Teleporters Teleporters;

    private void Awake()
    {
        Player = GetComponent<Player>();
        Teleporters = GetComponent<Teleporters>();
    }
    /// <summary>
    /// Moves the Player.Transform via Lerp(), checks if the Player is grounded and if it collides on any side or upwards, then applies the PlayerAttributes.MoveSpeedMultiplier
    /// </summary>
    /// <param name="yVelocity"></param>
    /// <param name="xVelocity"></param>
    public void ExecuteMove(float yVelocity, float xVelocity)
    {
        if (Player.PlayerAttributes.Grounded)
        {
            if (Player.PlayerAttributes.MovingLeft && Player.PlayerAttributes.CollidingLeft)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                transform.position.y, transform.position.z);
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
            else if (Player.PlayerAttributes.MovingRight && Player.PlayerAttributes.CollidingRight)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                transform.position.y, transform.position.z);
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
            else // whenever the Player is moving in a direction where it is not colliding the x.position will change
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + xVelocity, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                transform.position.y, transform.position.z);
            }
        }
        else // In-air movement methods
        {
            if (Player.PlayerAttributes.CollidingUp) // Stops the y.position from changing upwards while still enabling x.position movement 
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + xVelocity, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                Mathf.Lerp(transform.position.y,
                transform.position.y, Player.PlayerAttributes.VerticalForceMultiplier * Time.deltaTime), transform.position.z);
            }
            if (Player.PlayerAttributes.MovingLeft && Player.PlayerAttributes.CollidingLeft) // Stops the Player from moving to the left if it is colliding on the left side while enabling the Player to move on the y-axis while not grounded
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                Mathf.Lerp(transform.position.y, transform.position.y + yVelocity, Player.PlayerAttributes.VerticalForceMultiplier * Time.deltaTime), transform.position.z);
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
            else if (Player.PlayerAttributes.MovingRight && Player.PlayerAttributes.CollidingRight) // Stops the Player from moving to the right if it is colliding on the right side while enabling the Player to move on the y-axis while not grounded
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                Mathf.Lerp(transform.position.y, transform.position.y + yVelocity, Player.PlayerAttributes.VerticalForceMultiplier * Time.deltaTime), transform.position.z);
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
            else // Moves the Player on both the x and the y axis
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + xVelocity, Player.PlayerAttributes.MovespeedMultiplier * Time.deltaTime),
                Mathf.Lerp(transform.position.y, transform.position.y + yVelocity, Player.PlayerAttributes.VerticalForceMultiplier * Time.deltaTime), transform.position.z);
            }
        }
    }
    /// <summary>
    /// Determines the base PlayerVelocity which is sent to the ExecuteMove() depending on if the player is walking, running or standing still 
    /// </summary>
    public void XAxisMoveCalc()
    {
        if (Player.PlayerAttributes.Walking && !Player.PlayerAttributes.Running) // walking
        {
            Player.PlayerAttributes.PlayerVelocity += Time.deltaTime / Player.PlayerAttributes.MoveDivider;
            if (Player.PlayerAttributes.PlayerVelocity > Player.PlayerAttributes.MaxspeedWalk)
            {
                Player.PlayerAttributes.PlayerVelocity = Player.PlayerAttributes.MaxspeedWalk;
            }
        }
        else if (Player.PlayerAttributes.Running) // running
        {
            Player.PlayerAttributes.PlayerVelocity += (Time.deltaTime * Player.PlayerAttributes.RunspeedMultiplier) / Player.PlayerAttributes.MoveDivider;
            if (Player.PlayerAttributes.PlayerVelocity > Player.PlayerAttributes.MaxspeedRun)
            {
                Player.PlayerAttributes.PlayerVelocity = Player.PlayerAttributes.MaxspeedRun;
            }
        }
        else if (!Player.PlayerAttributes.Walking && !Player.PlayerAttributes.Running && Player.PlayerAttributes.Grounded) // standing still
        {
            Player.PlayerAttributes.PlayerVelocity = 0;
        }
    }
    /// <summary>
    /// Determines the base GravitationalForce while jumping which is sent to the ExecuteMove() depending on 
    /// </summary>
    public void YAxisMoveCalc()
    {
        if (Player.PlayerAttributes.Jumping && Player.PlayerAttributes.CanJump)
        {
            if (Player.PlayerAttributes.CollidingUp)
            {
                Player.PlayerAttributes.GravitationalForce = 0;
            }
            else
            {
                Player.PlayerAttributes.GravitationalForce = Player.PlayerAttributes.JumpVelocity + Player.PlayerAttributes.JumpStartingVelocity / 5 + (Player.PlayerAttributes.CounterJumptime * 1.20f);
            }
            if (Player.PlayerAttributes.GravitationalForce > Player.PlayerAttributes.MaxspeedJump)
            {
                Player.PlayerAttributes.GravitationalForce = Player.PlayerAttributes.MaxspeedJump;
            }
            Player.PlayerAttributes.CounterJumptime += Time.deltaTime;
            if (Player.PlayerAttributes.CounterJumptime >= Player.PlayerAttributes.CounterJumptimeSeconds) // removes the players ability to jump once the jump has lasted for CounterJumpTimeSeconds
            {
                Player.PlayerAttributes.CanJump = false;
                Player.PlayerAttributes.CounterJumptime = 0;
            }
        }
    }
    /// <summary>
    /// Handles the Players PlayerVelocity(x-axis) while the player is in the air, makes sure that the player cannot change direction in air and 
    /// </summary>
    public void CalculateInAirXVelocity()
    {
        // caps out the players x velocity in the air in the cases where the player changes direction while in the air or is moving below DirectionChangedMaxAirSpeed value before the jump
        if (!Player.PlayerAttributes.Grounded && Player.PlayerAttributes.JumpStartingVelocity < Player.PlayerAttributes.DirectionChangedMaxAirSpeed && 
            Player.PlayerAttributes.PlayerVelocity > Player.PlayerAttributes.DirectionChangedMaxAirSpeed)
        {
            Player.PlayerAttributes.PlayerVelocity = Player.PlayerAttributes.DirectionChangedMaxAirSpeed;
        }
        else if (!Player.PlayerAttributes.Grounded && Player.PlayerAttributes.PlayerVelocity > Player.PlayerAttributes.JumpStartingVelocity && // maintains the PlayerVelocity in the air as long as the player is not changing direction
            Player.PlayerAttributes.JumpStartingVelocity != 0)
        {
            Player.PlayerAttributes.PlayerVelocity = Player.PlayerAttributes.JumpStartingVelocity;
        }
    }
    /// <summary>
    /// Applies a negative value to GravitationalForce if the player is not grounded and not jumping
    /// </summary>
    public void ApplyGravity()
    {
        if (!Player.PlayerAttributes.Grounded && !Player.PlayerAttributes.Jumping)
        {
            Player.PlayerAttributes.GravitationalForce -= Time.deltaTime * 1.25f;
            if (Player.PlayerAttributes.GravitationalForce < Player.PlayerAttributes.MaxspeedGravity)
            {
                Player.PlayerAttributes.GravitationalForce = Player.PlayerAttributes.MaxspeedGravity; // caps the GravitationalForce
            }
            Player.PlayerAttributes.CanJump = false;
        }
    }
    /// <summary>
    /// Is called from the InputManager, sets the moving and facing values aswell as enabling the CanWallJump variable and timer when changing direction away from a wall
    /// </summary>
    /// <param name="input"></param>
    public void ProcessMove(float input)
    {
        if (input > 0)
        {
            if (Player.PlayerAttributes.CollidingLeft && !Player.PlayerAttributes.Jumping)
            {
                Player.PlayerAttributes.CanWalljump = true;
                Player.PlayerAttributes.WalljumpCooldown = Player.PlayerAttributes.WalljumpCooldownSeconds;
            }
            Player.PlayerAttributes.MovingRight = true;
            Player.PlayerAttributes.FacingRight = true;
            if (Player.PlayerAttributes.MovingLeft)
            {
                Player.PlayerAttributes.MovingLeft = false;
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
        }
        else if (input < 0)
        {
            if (Player.PlayerAttributes.CollidingRight && !Player.PlayerAttributes.Jumping)
            {
                Player.PlayerAttributes.CanWalljump = true;
                Player.PlayerAttributes.WalljumpCooldown = Player.PlayerAttributes.WalljumpCooldownSeconds;
            }
            Player.PlayerAttributes.MovingLeft = true;
            Player.PlayerAttributes.FacingRight = false;
            if (Player.PlayerAttributes.MovingRight)
            {
                Player.PlayerAttributes.MovingRight = false;
                Player.PlayerAttributes.PlayerVelocity = 0;
            }
        }
        if (input != 0)
        {
            Player.PlayerAttributes.Walking = true;
        }
        else
        {
            Player.PlayerAttributes.JumpStartingVelocity = 0;
            Player.PlayerAttributes.Walking = false;
            Player.PlayerAttributes.MovingLeft = false;
            Player.PlayerAttributes.MovingRight = false;
        }
        Player.PlayerAttributes.MoveDirection = Player.PlayerAttributes.PlayerVelocity * input; // MoveDirection is used in Player.Update() when the ExecuteMove() is called setting the actual direction and velocity of the Players x-axis 
    }
    /// <summary>
    /// Is called from the InputManager, makes the character jump or walljump if the criteria is met
    /// </summary>
    /// <param name="isJumping"></param>
    public void ProcessJump(bool isJumping)
    {
        if (!Player.PlayerAttributes.Jumping && !isJumping && !Player.PlayerAttributes.Grounded && Player.PlayerAttributes.CanWalljump) // enables walljumping if the player is not Jumping, Grounded, CanWallJump and the input of jump is false
        {
            Player.PlayerAttributes.WalljumpsLeft = 1;
        }
        if (isJumping && Player.PlayerAttributes.CanWalljump && !Player.PlayerAttributes.Grounded && !Player.PlayerAttributes.Jumping && Player.PlayerAttributes.WalljumpsLeft > 0) // starts the walljump with fixed velocity and enables the player to keep jumping to gain y-velocity
        {
            Player.PlayerAttributes.CanJump = true;
            Player.PlayerAttributes.CanWalljump = false;
            Player.PlayerAttributes.CounterJumptime = Player.PlayerAttributes.CounterJumptimeSeconds / 2;
            Player.PlayerAttributes.PlayerVelocity = Player.PlayerAttributes.MaxspeedRun / 2f;
            Player.PlayerAttributes.JumpStartingVelocity = Player.PlayerAttributes.PlayerVelocity;
            Player.PlayerAttributes.WalljumpsLeft = 0;
        }
        if (isJumping && Player.PlayerAttributes.CanJump && Player.PlayerAttributes.NumberOfJumps < 2)
        {
            Player.PlayerAttributes.Jumping = true;
            if (Player.PlayerAttributes.Grounded)
            {
                Player.PlayerAttributes.JumpStartingVelocity = Player.PlayerAttributes.PlayerVelocity;
                Player.PlayerAttributes.NumberOfJumps++;
                Player.PlayerAttributes.Grounded = false;
            }
        }
        else
        {
            Player.PlayerAttributes.Jumping = false;
        }
        if (!isJumping && Player.PlayerAttributes.Grounded)
        {
            Player.PlayerAttributes.NumberOfJumps = 0; // the NumberOfJumps variable makes sure that the Player cannot continue to jump when they land unless they release the jumpbutton first
        }
    }
    public void ToggleRun(float isRunning)
    {
        if (isRunning > 0 && Player.PlayerAttributes.Walking)
        {
            Player.PlayerAttributes.Running = true;
        }
        else
        {
            Player.PlayerAttributes.Running = false;
        }
    }
    public void Teleport(bool value)
    {
        if (Player.PlayerAttributes.CanActivateTeleporter && value)
        {
            Teleporters.ActivateTeleporter(this.GetComponent<Player>());
        }
    }
    public void WalljumpTimer()
    {
        if (Player.PlayerAttributes.WalljumpCooldown > 0)
        {
            Player.PlayerAttributes.WalljumpCooldown -= Time.deltaTime;
            if (Player.PlayerAttributes.WalljumpCooldown <= 0.0f)
            {
                Player.PlayerAttributes.CanWalljump = false;
                Player.PlayerAttributes.WalljumpCooldown = 0;
            }
        }
    }
}
