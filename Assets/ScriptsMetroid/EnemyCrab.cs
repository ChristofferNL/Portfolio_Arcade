using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCrab : Enemy
{
    public Transform RayPosLeft, RayPosDown;
    public override int Hitpoints { get; set; } = 4;
    public override Animator Animator { get; set; }
    public override SpriteRenderer SpriteRenderer { get; set; }
    public override Dictionary<ObjectStates, string> AnimationValuePairs { get; set; } = new Dictionary<ObjectStates, string>()
    {
        { ObjectStates.Idle, "Crab_Idle"},
        { ObjectStates.Walking, "Crab_Walking"},
        { ObjectStates.Death, "Crab_Death"}
    };
    public override int DamageDealt { get; set; } = 5;
    public override bool Destroyed { get; set; }
    public override ObjectStates ActiveState { get; set; }

    public float MoveSpeed;
    public bool MovingLeft;
    public bool CanTurnDown;
    public bool CanTurnUp;

    private float _zFixedPos = 48;  
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        ActiveState = ObjectStates.Walking;
        if (!MovingLeft)
        {
            transform.Rotate(0, 180.0000f, 0);
        }
    }
    private void Update()
    {
        CheckAnimationState();
        Raycasts();
        MoveCrab();
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
    /// Moves the object through a Translate() either left or right depending on the value of the movingLeft boolean
    /// </summary>
    private void MoveCrab()
    {
        if (!Destroyed && MovingLeft) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zFixedPos);
            transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
        }else if (!Destroyed && !MovingLeft)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zFixedPos);
            transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
        }
    }
    /// <summary>
    /// One raycast in front of the object going forward making sure that the object turns 90 degrees before hitting an object tagged as WorldMaterial, and one raycast 
    /// behind the object going downwards making sure that the object turns 90 degrees once its no longer moving on top of a object tagged as WorldMaterial
    /// </summary>
    private void Raycasts()
    {
        if (Physics.Raycast(RayPosDown.position, transform.TransformDirection(Vector3.down), out RaycastHit DownHit, 1f))
        {
            if (DownHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                CanTurnDown = true; //once this bool is true the object can once again turn downwards to avoid an infinite rotation
            }
        }
        else
        {
            if (CanTurnDown)
            {
                transform.Rotate(transform.rotation.z, 0, 90f, Space.Self);
                CanTurnDown = false;
            }
        }
        if (Physics.Raycast(RayPosLeft.position, transform.TransformDirection(Vector3.left), out RaycastHit LeftHit, 0.1f))
        {
            if (LeftHit.transform.gameObject.CompareTag("WorldMaterial"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, _zFixedPos);
                transform.Rotate(0, 0, -90.000f);
            }
        }
    }
}
