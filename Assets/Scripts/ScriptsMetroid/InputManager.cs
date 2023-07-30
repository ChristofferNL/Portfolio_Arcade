using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFoot;
    private PlayerInput.InAirActions _inAir;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    public bool MovementEnabled;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _onFoot = _playerInput.OnFoot;
        _inAir = _playerInput.InAir;
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        MovementEnabled = true; // this can be turned to false to disable player inputs, is used during the teleport 
    }
    private void OnEnable()
    {
        _onFoot.Enable();
        _inAir.Enable();
    }
    private void OnDisable()
    {
        _onFoot.Disable();
        _inAir.Disable();
    }
    private void Update()
    {
        if (MovementEnabled)
        {
            _playerAttack.ProcessShoot(_onFoot.Shoot.IsPressed());
            _playerMove.ProcessMove(_onFoot.Movement.ReadValue<float>());
            _playerMove.ToggleRun(_onFoot.Run.ReadValue<float>());
            _playerMove.ProcessJump(_onFoot.Jump.IsPressed());
            _playerMove.Teleport(_onFoot.Activate.IsPressed());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
    }
}
