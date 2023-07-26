using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates the object
/// </summary>
public class RotatePickUpObject : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private ParticleSystem glowEffect;
    [SerializeField] private float effectPulseTime;
    private float effectTimer;

    private void Awake()
    {
        effectTimer = Random.Range(0, 3);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
        //effectTimer += Time.deltaTime;
        //if (effectTimer >= effectPulseTime)
        //{
        //    ParticleSystem clone = Instantiate(glowEffect, transform.position, Quaternion.identity);
        //    clone.transform.SetParent(this.transform);
        //    effectTimer = 0;
        //}
    }
}
