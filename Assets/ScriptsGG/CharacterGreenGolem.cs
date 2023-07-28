using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGreenGolem : Character
{
    public override Animator Animator { get; set; }

    public ParticleSystem AttackEffectParticle;

    public override void CreateAttackHitbox()
    {
        GameObject clone = Instantiate(GolemDataObject.AttackProjectileObject, attackSpawnLocation.position, Quaternion.identity);
        EventManagerGG.Instance.ParticlePlayEvent(AttackEffectParticle, transform.position);
    }
}
