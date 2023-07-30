using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GolemTypeData")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Movement")]
    public float MoveSpeedMultiplier;
    public float MaxMoveSpeed;
    public float JumpDuration;
    public float JumpSpeed;

    [Header("Attack")]
    public float AttackCooldown;

    public GameObject AttackProjectileObject;

    public AnimationClip IdleAnim;
    public AnimationClip WalkAnim;
    public AnimationClip AttackAnim;

    public AudioClip AttackSoundEffekt;
}
