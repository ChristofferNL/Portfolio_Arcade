using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Keeps track of if the player is colliding with any obstacles, adding a penalty to the player score while the player is colliding with any obstacle, also creates the particle effect
/// </summary>
public class PlayerCollisionBirdGame : MonoBehaviour
{
    [SerializeField] private ParticleSystem collisionEffect;
    private float ruffledFeathers;
    [SerializeField] private TextMeshProUGUI penaltyText;

    private void Awake()
    {
        UpdatePenaltyText();
    }

    private void UpdatePenaltyText()
    {
        penaltyText.text = $"- {(int)(ruffledFeathers * 1500)}";
    }

    public float GetRuffledFeatherValue()
    {
        UpdatePenaltyText();
        return ruffledFeathers;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ruffledFeathers += Time.deltaTime * 2;
            ParticleSystem clone = Instantiate(collisionEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.05f), Quaternion.identity);
            clone.transform.SetParent(transform);
            UpdatePenaltyText();
        }
    }
}
