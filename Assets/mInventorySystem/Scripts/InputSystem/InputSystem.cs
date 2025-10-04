using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : IInputSystem
{
    private readonly GameInputs _gameInputs;

    public Vector2 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
    
    public event Action JumpChanged;
    public event Action InteractChanged;
    public event Action UseChanged;
    public event Action DropChanged;
    
    public event Action InventoryViewChanged;

    public InputSystem()
    {
        _gameInputs = new GameInputs();
        
        _gameInputs.Enable();

        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;
        
        _gameInputs.Player.Look.performed += OnLook;
        _gameInputs.Player.Look.canceled += OnLook;
        
        _gameInputs.Player.Jump.performed += OnJump;
        
        _gameInputs.Player.Use.performed += OnUse;
        _gameInputs.Player.Drop.performed += OnDrop;
        _gameInputs.Player.Interact.performed += OnInteract;
        
        _gameInputs.UI.Inventory.performed += OnInventory;
    }
    
    public void EnablePlayerMap() => _gameInputs.Player.Enable();

    public void DisablePlayerMap() => _gameInputs.Player.Disable();

    private void OnMove(InputAction.CallbackContext ctx) => MoveDirection = ctx.ReadValue<Vector2>();

    private void OnLook(InputAction.CallbackContext ctx) => LookDirection = ctx.ReadValue<Vector2>();
    
    private void OnJump(InputAction.CallbackContext ctx) => JumpChanged?.Invoke();
    
    private void OnUse(InputAction.CallbackContext obj) => UseChanged?.Invoke();
    private void OnDrop(InputAction.CallbackContext obj) => DropChanged?.Invoke();
    private void OnInteract(InputAction.CallbackContext obj) => InteractChanged?.Invoke();
    
    private void OnInventory(InputAction.CallbackContext obj) => InventoryViewChanged?.Invoke();

    public void Dispose()
    {
        _gameInputs.Player.Move.performed -= OnMove;
        _gameInputs.Player.Move.canceled -= OnMove;

        _gameInputs.Player.Look.performed -= OnLook;
        _gameInputs.Player.Look.canceled -= OnLook;

        _gameInputs.Player.Jump.performed -= OnJump;
        
        _gameInputs.Player.Use.performed -= OnUse;
        _gameInputs.Player.Drop.performed -= OnDrop;
        _gameInputs.Player.Interact.performed -= OnInteract;
        
        _gameInputs.UI.Inventory.performed -= OnInventory;

        _gameInputs.Disable();
        _gameInputs.Dispose();
    }
}