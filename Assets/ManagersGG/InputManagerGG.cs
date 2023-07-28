using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerGG : MonoBehaviour
{
    private PlayerControlsGG inputActions;
    private PlayerControlsGG.PlayerActionMapActions playerActions;
    public FixedJoystick FixedJoystick;

    private void Awake()
    {
        inputActions = new();
        playerActions = inputActions.PlayerActionMap;
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Update()
    {
        EventManagerGG.Instance.MoveInputEvent(FixedJoystick.Horizontal);
        //EventManager.Instance.AttackInputEvent(playerActions.Attack.IsPressed());
        EventManagerGG.Instance.MoveInputEvent(playerActions.MovementActions.ReadValue<float>());
        EventManagerGG.Instance.JumpInputEvent(playerActions.Jump.IsPressed());
    }
}
