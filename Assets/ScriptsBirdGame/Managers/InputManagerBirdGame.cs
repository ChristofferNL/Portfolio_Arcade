using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Input manager using the new input system in Unity, creating event calls for the event manager 
/// </summary>
public class InputManagerBirdGame : MonoBehaviour
{
    private PlayerControls inputActions;
    private PlayerControls.PlayerAMActions actions;

    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private Button FlapButton;

    public bool UsingKeyboard;

    private void Awake()
    {
        inputActions = new();
        actions = inputActions.PlayerAM;
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void Update()
    {
        if (UsingKeyboard)
        {
            EventManager.Instance.CallMoveEvent(actions.Move.ReadValue<float>());
            EventManager.Instance.CallFlapEvent(actions.Flap.IsPressed());
            EventManager.Instance.CallMoveEvent(fixedJoystick.Horizontal);
        }
        else
        {
            EventManager.Instance.CallMoveEvent(floatingJoystick.Horizontal);
        }
    }
}
