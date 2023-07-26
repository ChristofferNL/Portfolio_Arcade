using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Singleton static EventManager invokes events for all sounds via the SoundManager and the PlayerHP GUI via the UIManager and the MonsterManager methods
/// </summary>
public class EventManagerGlobal : MonoBehaviour
{
    public static EventManagerGlobal Instance { get; private set; }
    public UnityEvent EventSoundExplosion;
    public UnityEvent EventSoundShoot;
    public UnityEvent<int> EventTakeDamage;
    public UnityEvent EventSoundLowLife;
    public UnityEvent EventSoundRotateWorld;
    public UnityEvent<int> EventMonsterActivate;
    public UnityEvent EventMonsterDeActivate;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(this);
        }
    }
    public void MonsterActivate(int direction)
    {
        EventMonsterActivate.Invoke(direction);
    }
    public void MonsterDeactivate()
    {
        EventMonsterDeActivate.Invoke();
    }
    public void SoundExplosion()
    {
        EventSoundExplosion.Invoke();
    }
    public void SoundShoot()
    {
        EventSoundShoot.Invoke();
    }
    public void SoundLowLife()
    {
        EventSoundLowLife.Invoke();
    }
    public void SoundRotateWorld()
    {
        EventSoundRotateWorld.Invoke();
    }
    /// <summary>
    /// This method connects the Player.TakeDamage() with the UI and the sounds for taking damage aswell as the lowlife alert sound
    /// </summary>
    /// <param name="health"></param>
    public void TakeDamage(int health)
    {
        EventTakeDamage.Invoke(health);
        if (health < 21)
        {
            EventSoundLowLife.Invoke();
        }
    }
}
