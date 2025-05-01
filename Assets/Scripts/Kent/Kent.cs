using UnityEngine;

[SelectionBase]

public class Kent : MonoBehaviour
{
    public static Kent Instance { get; private set; }
    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private float _footstepSpeed = 0.3f; // Швидкість відтворення звуків кроків
    Vector2 inputVector;

    private Rigidbody2D _rb;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private Vector2 _lastMovementDirection;
    private KentVisual _kentVisual;
    private bool _playingFootsteps = false;

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
        HandleFootsteps();
    }

    private void HandleMovement()
    {
        if (PauseController.IsGamePaused)
        {
            _rb.linearVelocity = Vector2.zero;
            StopFootsteps();
            return;
        }

        // Використовуємо Velocity замість MovePosition для більш плавного руху
        _rb.linearVelocity = inputVector * _movingSpeed;

        // Зберігаємо напрямок руху, якщо він не нульовий
        if (inputVector.magnitude > _minMovingSpeed)
        {
            _lastMovementDirection = inputVector;
        }

        _isRunning = inputVector.magnitude > _minMovingSpeed;
    }

    private void HandleFootsteps()
    {
        if (_isRunning && !_playingFootsteps && !PauseController.IsGamePaused)
        {
            StartFootsteps();
        }
        else if (!_isRunning || PauseController.IsGamePaused)
        {
            StopFootsteps();
        }
    }

    private void StartFootsteps()
    {
        _playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, _footstepSpeed);
    }

    private void StopFootsteps()
    {
        if (_playingFootsteps)
        {
            _playingFootsteps = false;
            CancelInvoke(nameof(PlayFootstep));
        }
    }

    private void PlayFootstep()
    {
        SoundEffectManager.Play("Footstep", true);
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
