using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Holds all methods and audiosources to play sounds and music
/// </summary>
public class SoundManager : MonoBehaviour
{
    public AudioListener AudioListener;
    public AudioSource LevelMusic; //Open gameart, by Wolfgang, CC0, https://opengameart.org/content/8-bit-cave-loop
    public AudioSource PlayerShootSound; // Open gameart, by dklonm, CC-BY 3.0, https://opengameart.org/content/laser-fire
    public AudioSource ExplosionSound; // Open Gameart, by Luke.RUSTLTD, CC0, https://opengameart.org/content/bombexplosion8bit
    public AudioSource LowLifeSound; // Open Gameart, by LittleRobotSoundFactory, CC-BY 3.0, www.littlerobotsoundfactory.com
    public AudioSource TakingDamageSound; // Open Gameart, by LittleRobotSoundFactory, CC-BY 3.0, www.littlerobotsoundfactory.com
    public AudioSource RotateTheWorld; // Open Gameart, by LittleRobotSoundFactory, CC-BY 3.0, www.littlerobotsoundfactory.com


    void Awake()
    {
        LevelMusic.Play();
        EventManagerGlobal.Instance.EventSoundExplosion.AddListener(PlayEnemyDeathSound);
        EventManagerGlobal.Instance.EventSoundShoot.AddListener(PlayPlayerShootSound);
        EventManagerGlobal.Instance.EventTakeDamage.AddListener(PlayDamageTakenSound);
        EventManagerGlobal.Instance.EventSoundLowLife.AddListener(PlayLowLifeSound);
        EventManagerGlobal.Instance.EventSoundRotateWorld.AddListener(PlayRotateWorldSound);
    }
    public void PlayDamageTakenSound()
    {
        TakingDamageSound.Play();
    }
    public void PlayDamageTakenSound(int value)
    {
        TakingDamageSound.Play();
    }
    public void PlayPlayerShootSound()
    {
        PlayerShootSound.Play();
    }
    public void PlayEnemyDeathSound()
    {
        ExplosionSound.Play();
    }
    public void PlayLowLifeSound()
    {
        LowLifeSound.Play();
    }
    public void PlayRotateWorldSound()
    {
        if (RotateTheWorld.isPlaying)
        {
            RotateTheWorld.Stop();
        }
        else
        {
            RotateTheWorld.Play();
        }   
    }
}
