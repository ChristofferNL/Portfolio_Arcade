using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the actual movement of the player gameObject aswell as setting the animation state 
/// </summary>

public class PlayerController : AnimatedObjectBirdGame
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float flapSpeed;
    [SerializeField] private float maxFlapSpeed;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private bool isMoving;

    [SerializeField] private AnimationClip IdleAnim;
    [SerializeField] private AnimationClip MovingAnim;

    public override Animator Animator { get; set; }
    public override Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        Animator = GetComponentInChildren<Animator>();
        EventManager.Instance.EventMoveInput.AddListener(ExecuteMove);
        EventManager.Instance.EventFlapInput.AddListener(ExecuteFlap);

        AnimationValuePairs = new Dictionary<ObjectStates, AnimationClip>()
        {
            {ObjectStates.Idle, IdleAnim },
            {ObjectStates.Walking, MovingAnim }
        };
    }

    private void FixedUpdate()
    {
        CheckAnimationState();
    }

    private void ExecuteMove(float input)
    {
        if (input != 0)
        {
            isMoving = true;
            ActiveState = ObjectStates.Walking;
            rb.AddForce(new Vector3(input * moveSpeed * Time.deltaTime, 0, 0));
            if (rb.velocity.x > maxMoveSpeed)
            {
                rb.velocity = new Vector3(maxMoveSpeed, rb.velocity.y, 0);
            }
            else if (rb.velocity.x < - maxMoveSpeed)
            {
                rb.velocity = new Vector3(-maxMoveSpeed, rb.velocity.y, 0);
            }
            if (input > 0 && sr.flipX == true)
            {
                sr.flipX = false;
            }
            if (input < 0 && sr.flipX == false)
            {
                sr.flipX = true;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void ExecuteFlap(bool isPressed)
    {
        if (isPressed)
        {
            rb.AddForce(new Vector3(0, flapSpeed * Time.deltaTime, 0));
            ActiveState = ObjectStates.Walking;
            if (rb.velocity.y > maxFlapSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxFlapSpeed, 0);
            }
        }
        else if(!isMoving && !isPressed)
        {
            ActiveState = ObjectStates.Idle;
        }
    }
}

