using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GolemBuffData")]
public class CharacterBuffSO : ScriptableObject
{
    public ParticleSystem BuffEffect;
    public float ParticleYOffset;

    public float MoveSpeedModifier;
    public float JumpSpeedModifier;

    public bool OnLanding;
    public bool IsStatic;

    public AudioClip BuffEffectSound;
}
