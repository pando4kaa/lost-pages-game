using UnityEngine;

[SelectionBase]

public class Kent : MonoBehaviour
{
    public static Kent Instance { get; private set; }
    [SerializeField] private float _movingSpeed = 5f;
    Vector2 inputVector;

    private Rigidbody2D _rb;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private Vector2 _lastMovementDirection;
    private KentVisual _kentVisual;

    private PolygonCollider2D _polygonCollider;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _kentVisual = GetComponentInChildren<KentVisual>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnKentAttack += Kent_OnKentAttack;
        AttackColliderTurnOff();
    }

    private void Kent_OnKentAttack(object sender, System.EventArgs e)
    {
        _kentVisual.PlayAttackAnimation();
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
        // Використовуємо Velocity замість MovePosition для більш плавного руху
        _rb.linearVelocity = inputVector * _movingSpeed;

        // Зберігаємо напрямок руху, якщо він не нульовий
        if (inputVector.magnitude > _minMovingSpeed)
        {
            _lastMovementDirection = inputVector;
        }

        _isRunning = inputVector.magnitude > _minMovingSpeed;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public bool IsFacingRight()
    {
        return _lastMovementDirection.x >= 0;
    }

    public Vector3 GetKentScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void AttackColliderTurnOff()
    {
        _polygonCollider.enabled = false;
    }

    public void AttackColliderTurnOn()
    {
        _polygonCollider.enabled = true;
    }
}
