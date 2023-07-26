using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Puts a reference of all monsters on each side into an array, contains methods to activate the monsters on the current side of the level and method to freeze all monsters while the world rotates
/// </summary>
public class MonsterManager : MonoBehaviour
{
    public List<Enemy> EnemiesNorth;
    public List<Enemy> EnemiesSouth;
    public List<Enemy> EnemiesWest;
    public List<Enemy> EnemiesEast;

    private void Awake()
    {
        EventManagerGlobal.Instance.EventMonsterActivate.AddListener(ActivateMonsters);
        EventManagerGlobal.Instance.EventMonsterDeActivate.AddListener(DeactivateMonsters);
    }
    /// <summary>
    /// Activates all monsters on the active side of the level
    /// </summary>
    /// <param name="directionEnum"></param>
    public void ActivateMonsters(int directionEnum)
    {
        foreach (Enemy enemy in EnemiesNorth)
        {
            if (directionEnum == 0 && enemy != null)
            {
                enemy.enabled = true;
            }
            else if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesEast)
        {
            if (directionEnum == 1 && enemy != null)
            {
                enemy.enabled = true;
            }
            else if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesSouth)
        {
            if (directionEnum == 2 && enemy != null)
            {
                enemy.enabled = true;
            }
            else if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesWest)
        {
            if (directionEnum == 3 && enemy != null)
            {
                enemy.enabled = true;
            }
            else if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
    }
    /// <summary>
    /// Deactivates all monsters until the ActivateMonsters() is called
    /// </summary>
    public void DeactivateMonsters()
    {
        foreach (Enemy enemy in EnemiesNorth)
        {
            if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesEast)
        {
            if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesSouth)
        {
            if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
        foreach (Enemy enemy in EnemiesWest)
        {
            if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
    }
}
