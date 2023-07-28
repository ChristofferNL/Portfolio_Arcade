using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrucibleObject : MonoBehaviour
{
    public int HitPoints;

    public ParticleSystem DestroyedParticleEffect;
    public ParticleSystem HitParticleEffect;

    public AudioClip DestroyedSoundEffect;
    public AudioClip GettingHitSoundEffect;

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void TakeDamage(Vector3 hitPosition)
    {
        HitPoints--;
        if (HitPoints <= 0)
        {
            SoundManagerGG.Instance.PlaySoundEffect(GettingHitSoundEffect, true);
            SoundManagerGG.Instance.PlaySoundEffect(DestroyedSoundEffect, true);
            EventManagerGG.Instance.ParticlePlayEvent(DestroyedParticleEffect, transform.position);
            SaveManager.Instance.RecordBuildingDestroyed();
            Die();
        }
        else
        {
            SoundManagerGG.Instance.PlaySoundEffect(GettingHitSoundEffect, true);
            EventManagerGG.Instance.ParticlePlayEvent(HitParticleEffect, hitPosition);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            TakeDamage(collision.contacts[0].point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            TakeDamage(other.ClosestPoint(transform.position));
        }
    }
}
