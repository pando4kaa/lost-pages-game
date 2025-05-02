using UnityEngine;
using System.Linq;

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
    private bool _isAttacking = false;

    private PolygonCollider2D _attackCollider;
    private Vector2[] _originalAttackPath;
    [SerializeField] private int _damage = 1;


    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _kentVisual = GetComponentInChildren<KentVisual>();
        _attackCollider = GetComponent<PolygonCollider2D>();
        _originalAttackPath = _attackCollider.GetPath(0);
    }

    private void Start()
    {
        GameInput.Instance.OnKentAttack += Kent_OnKentAttack;
        AttackColliderTurnOff();
    }

    private void Kent_OnKentAttack(object sender, System.EventArgs e)
    {
        if (_isAttacking) return;
        _isAttacking = true;
        AttackColliderTurnOffOn();
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
        if (PauseController.IsGamePaused || _isAttacking)
        {
            _rb.linearVelocity = Vector2.zero;
            StopFootsteps();
            _isRunning = false;
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
        if (_isRunning && !_playingFootsteps && !PauseController.IsGamePaused && !_isAttacking)
        {
            StartFootsteps();
        }
        else if (!_isRunning || PauseController.IsGamePaused || _isAttacking)
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

    public void Attack()
    {
        if (_isAttacking) return;
        AttackColliderTurnOffOn();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.TryGetComponent(out EnemyEntity enemy) && _isAttacking && _attackCollider.enabled)
        {
            enemy.Damage(_damage);
        }
    }

    public void AttackColliderTurnOff()
    {
        _attackCollider.enabled = false;
    }

    public void AttackColliderTurnOn()
    {
        _attackCollider.enabled = true;
    }
    public void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }

    public void EndAttack()
    {
        AttackColliderTurnOff();
        _isAttacking = false;
    }

    public void UpdateAttackColliderDirection(bool facingRight)
    {
        var newPath = facingRight ? _originalAttackPath : _originalAttackPath.Select(p => new Vector2(-p.x, p.y)).ToArray();
        _attackCollider.SetPath(0, newPath);
    }
}
