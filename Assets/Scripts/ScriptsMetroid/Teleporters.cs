using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Contains the methods for teleporting the player and rotating the level 
/// </summary>
public class Teleporters : MonoBehaviour
{
    public GameObject TheWorld;
    private Player _playerToTeleport;
    private Vector3 TeleportDestination;
    private Vector3 TeleportWaitPosition = new Vector3(0 ,9, 0);
    public float TimeToRotateWorld;
    public float TimeToRotateWorldSeconds;
    public bool RotateWorldLeft;
    public bool RotateWorldRight;
    public Directions FacingDirection;

    private void Start()
    {
        FacingDirection = Directions.North;
        EventManagerGlobal.Instance.MonsterActivate((int)FacingDirection);
    }

    public enum Directions
    {
        North, East, South, West
    }
    private void Update()
    {
        if (RotateWorldLeft)
        {
            RotateTheWorld(true);
            TimeToRotateWorld -= Time.deltaTime;
            _playerToTeleport.transform.position = TeleportWaitPosition;   
            if (TimeToRotateWorld <= 0.0f) // Stops the rotation and moves the player back onto the level
            {
                RotateWorldLeft = false;
                EventManagerGlobal.Instance.SoundRotateWorld();
                TimeToRotateWorld = TimeToRotateWorldSeconds;
                _playerToTeleport.transform.position = TeleportDestination;
                _playerToTeleport.GetComponent<InputManager>().MovementEnabled = true;
                _playerToTeleport.GetComponentInChildren<SpriteRenderer>().enabled = true;
                EventManagerGlobal.Instance.MonsterActivate((int)FacingDirection);
            }
        }
        if (RotateWorldRight)
        {
            RotateTheWorld(false);
            TimeToRotateWorld -= Time.deltaTime;
            _playerToTeleport.transform.position = TeleportWaitPosition;
            if (TimeToRotateWorld <= 0.0f) // Stops the rotation and moves the player back onto the level
            {
                RotateWorldRight = false;
                EventManagerGlobal.Instance.SoundRotateWorld();
                TimeToRotateWorld = TimeToRotateWorldSeconds;
                _playerToTeleport.transform.position = TeleportDestination;
                _playerToTeleport.GetComponent<InputManager>().MovementEnabled = true;
                _playerToTeleport.GetComponentInChildren<SpriteRenderer>().enabled = true;
                EventManagerGlobal.Instance.MonsterActivate((int)FacingDirection);
            }
        }
    }
    /// <summary>
    /// Rotates the level left or right depending on which teleport the player activated
    /// </summary>
    /// <param name="left"></param>
    public void RotateTheWorld(bool left)
    {
        if (left)
        {
            TheWorld.transform.Rotate(0, +Time.deltaTime * 18, 0);
        }
        if (!left)
        {
            TheWorld.transform.Rotate(0, +Time.deltaTime * -18, 0);
        }
    }
    /// <summary>
    /// Determines what direction the world will rotate and where the TeleportDestination will be, turns of the SpriteRenderer of the Player aswell as the ability to make inputs via the InputManager until the rotation is completed
    /// </summary>
    /// <param name="player"></param>
    public void ActivateTeleporter(Player player)
    {
        _playerToTeleport = player;
        if (player.transform.position.x < 0) // this is based on the fact that i know that x == 0 between the teleporters at any given time
        {
            // the distance between the teleporters is 8 and i want the player to appear in the other teleporter after the rotation to make them seem connected red to green
            TeleportDestination = new Vector3(_playerToTeleport.transform.position.x +8, _playerToTeleport.transform.position.y, _playerToTeleport.transform.position.z); 
            RotateWorldLeft = true;
            EventManagerGlobal.Instance.SoundRotateWorld();
            if (FacingDirection == Directions.North)
            {
                FacingDirection = Directions.West;
            }
            else
            {
                FacingDirection--;
            }
            EventManagerGlobal.Instance.MonsterDeactivate();
        }
        else
        {
            TeleportDestination = new Vector3(_playerToTeleport.transform.position.x - 8, _playerToTeleport.transform.position.y, _playerToTeleport.transform.position.z);
            RotateWorldRight = true;
            EventManagerGlobal.Instance.SoundRotateWorld();
            if (FacingDirection == Directions.West)
            {
                FacingDirection = Directions.North;
            }
            else
            {
                FacingDirection++;
            }
            EventManagerGlobal.Instance.MonsterDeactivate();
        }  
        _playerToTeleport.transform.position = TeleportWaitPosition;
        _playerToTeleport.GetComponent<InputManager>().MovementEnabled = false;
        _playerToTeleport.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }  
}
