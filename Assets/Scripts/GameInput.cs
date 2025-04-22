using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private KentInputAction _kentInputActions;
    public event EventHandler OnKentAttack;

    private void Awake()
    {
        Instance = this;
        _kentInputActions = new KentInputAction();
        _kentInputActions.Enable();

        _kentInputActions.Combat.Attack.started += KentAttack_started;
    }

    private void KentAttack_started(InputAction.CallbackContext context)
    {
        OnKentAttack?.Invoke(this, EventArgs.Empty);
    }
    
    public  Vector2 GetMovementVector()
    {
        Vector2 inputVector = _kentInputActions.Kent.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
     
}
