using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOctopus : Enemy
{
    public override int Hitpoints { get; set; } = 3;
    public override Animator Animator { get; set; }
    public override SpriteRenderer SpriteRenderer { get; set; }
    public override Dictionary<ObjectStates, string> AnimationValuePairs { get; set; } = new Dictionary<ObjectStates, string>() 
    {
        { ObjectStates.Idle, "Octopus_Idle"},
        { ObjectStates.Death, "Octopus_Death"}
    };
    public override int DamageDealt { get; set; } = 3;
    public override bool Destroyed { get; set; }
    public override ObjectStates ActiveState { get; set; }

    public float MoveDistance;
    public float MoveDistanceSeconds;
    public float MoveSpeed;
    public bool GoingLeft;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        ActiveState = ObjectStates.Idle;
    }
    private void Update()
    {
        CheckAnimationState();
        MoveOctopus();
    }
    /// <summary>
    /// Turns off interactions with this onject while the death animation runs
    /// </summary>
    public override void Death()
    {
        Destroyed = true;
        EventManagerGlobal.Instance.SoundExplosion();
        ActiveState = ObjectStates.Death;
        Destroy(this.gameObject, 0.35f);
    }
    /// <summary>
    /// When moveDistance becomes 0 the Octopus flips the SpriteRenderer, the Octopus is moved by a Translate()
    /// </summary>
    private void MoveOctopus()
    {
        MoveDistance -= Time.deltaTime;
        if (MoveDistance <= 0)
        {
            GoingLeft = !GoingLeft;
            MoveDistance = MoveDistanceSeconds;
        }
        if (GoingLeft && !Destroyed)
        {
            SpriteRenderer.flipX = false;
            transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed, Space.Self);
        }
        else if (!GoingLeft && !Destroyed)
        {
            SpriteRenderer.flipX = true;
            transform.Translate(Vector3.left * Time.deltaTime * -MoveSpeed, Space.Self);
        }
    }
}
