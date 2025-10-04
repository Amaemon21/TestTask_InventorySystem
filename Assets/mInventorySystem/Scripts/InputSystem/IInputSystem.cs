using System;
using UnityEngine;

public interface IInputSystem : IDisposable
{
    public Vector2 MoveDirection { get; }
    public Vector2 LookDirection { get; }
    
    public event Action JumpChanged;
    
    public void EnablePlayerMap();
    public void DisablePlayerMap();
}