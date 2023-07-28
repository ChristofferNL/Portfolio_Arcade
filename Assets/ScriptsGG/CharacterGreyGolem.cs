using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGreyGolem : Character
{
    public override Animator Animator { get; set; }
    public override void CreateAttackHitbox()
    {
        GameObject clone = Instantiate(GolemDataObject.AttackProjectileObject, attackSpawnLocation.position, Quaternion.identity);
        clone.GetComponent<InvisibleRock>().AdjustRockPosition(isMovingRight);
    }
}
