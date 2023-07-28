using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlackGolem : Character
{
    public override Animator Animator { get; set; }

    public override void CreateAttackHitbox()
    {
        GameObject clone = Instantiate(GolemDataObject.AttackProjectileObject, attackSpawnLocation.position, Quaternion.identity);
        clone.AddComponent<FlyingRock>();
        clone.GetComponent<FlyingRock>().SendRockFlying(isMovingRight, Rigidbody.velocity.x);
    }
}
