using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A prefab of this class is assigned to the PlayerAttack class, the direction the bullet travels is set by the PlayerAttack.ProcessShoot() 
/// </summary>
public class Bullet : MonoBehaviour
{
    public float Speed;
    public bool MovingRight;
    public int Damage = 1;
    private float _flightTime = 5f;
    private Animator _animator;
    private bool _destroyed;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Update() //Moving the bullet depending on set direction or destroying the bullet if the flightTime has been reached
    {
        if (MovingRight && !_destroyed)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + Speed, Time.deltaTime),
            transform.position.y, transform.position.z);
        }
        else if (!MovingRight && !_destroyed)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x - Speed, Time.deltaTime),
            transform.position.y, transform.position.z);
        }
        _flightTime -= Time.deltaTime;
        if (_flightTime <= 0.0f)
        {
            Destroy(this.gameObject, 0.2f);
            _animator.Play("Bullet_Explodes");
        }
    }
    private void OnTriggerEnter(Collider other) //Runs the TakeDamage method if an enemy collides with the bullet or destroys it if it collides with a object tagged as WorldMaterial
    {
        if (other.GetComponent<Enemy>() && !_destroyed)
        {
            _destroyed = true;
            Destroy(this.gameObject, 0.2f);
            _animator.Play("Bullet_Explodes");
            other.GetComponent<Enemy>().TakeDamage(Damage);
        }
        if (other.gameObject.tag == "WorldMaterial")
        {
            _destroyed = true;
            Destroy(this.gameObject, 0.2f);
            _animator.Play("Bullet_Explodes");
        }
    }
}
