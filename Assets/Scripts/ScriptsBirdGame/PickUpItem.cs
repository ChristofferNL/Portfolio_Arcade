using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is placed on every gameobject the player will be able to pick up by colliding with it
/// </summary>
public class PickUpItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem pickUpEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.CallPickUpItemEvent();
            ParticleSystem clone = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
