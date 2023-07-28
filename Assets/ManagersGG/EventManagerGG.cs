using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManagerGG : MonoBehaviour
{
    public static EventManagerGG Instance;
    public UnityEvent<float> EventMoveInput;
    public UnityEvent<bool> EventJumpInput;
    public UnityEvent<bool> EventAttackInput;
    public UnityEvent<ParticleSystem, Vector3> EventPlayParticle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ParticlePlayEvent(ParticleSystem particleSystem, Vector3 position)
    {
        EventPlayParticle.Invoke(particleSystem, position);
        ParticleSystem particle = Instantiate(particleSystem, position, Quaternion.identity);
    }

    public void AttackInputEvent(bool isAttacking)
    {
        EventAttackInput.Invoke(isAttacking);
    }

    public void MoveInputEvent(float input)
    {
        EventMoveInput.Invoke(input);
    }

    public void JumpInputEvent(bool isJumping)
    {
        EventJumpInput.Invoke(isJumping);
    }
}
