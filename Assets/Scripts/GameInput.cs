using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private KentInputAction kentInputActions;
    private void Awake()
    {
        Instance = this;
        kentInputActions = new KentInputAction();
        kentInputActions.Enable();
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
