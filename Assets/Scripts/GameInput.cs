using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private KentInputAction kentInputActions;
    public event EventHandler OnKentAttack;

    private void Awake()
    {
        Instance = this;
        kentInputActions = new KentInputAction();
        kentInputActions.Enable();

        kentInputActions.Combat.Attack.started += KentAttack_started;
    }

    private void KentAttack_started(InputAction.CallbackContext context)
    {
        OnKentAttack?.Invoke(this, EventArgs.Empty);
    }
    
    public  Vector2 GetMovementVector()
    {
        Vector2 inputVector = kentInputActions.Kent.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
     
}
