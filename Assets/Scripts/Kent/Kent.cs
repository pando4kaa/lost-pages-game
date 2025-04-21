using UnityEngine;

public class Kent : MonoBehaviour
{
    public static Kent Instance { get; private set; }
    [SerializeField] private float movingSpeed = 5f;
    Vector2 inputVector;

    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private Vector2 lastMovementDirection;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        // Зберігаємо напрямок руху, якщо він не нульовий
        if (inputVector.x != 0)
        {
            lastMovementDirection = inputVector;
        }

        isRunning = Mathf.Abs(inputVector.x) > minMovingSpeed || 
                    Mathf.Abs(inputVector.y) > minMovingSpeed;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsFacingRight()
    {
        return lastMovementDirection.x >= 0;
    }

    public Vector3 GetKentScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
}
