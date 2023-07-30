using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is placed on the nest gameObject, creating an event call while the player is colliding with the nets object
/// </summary>
public class NestScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.CallFinishRoundEvent();
        }
    }
}
