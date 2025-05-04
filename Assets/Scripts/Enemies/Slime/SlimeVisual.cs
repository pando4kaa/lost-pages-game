using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class SlimeVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private float _attackColliderDuration = 0.2f;  // duration collider is active during attack

    private Animator _animator;

    private const string IS_RUNNING = "IsRunning";
    private const string HURT = "Hurt";
    private const string IS_DIE = "IsDie";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_enemyAI != null)
            _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        if (_enemyEntity != null)
        {
            // disable collider by default
            _enemyEntity.PolygonColliderTurnOff();
            _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
            _enemyEntity.OnDeath += _enemyEntity_OnDeath;
        }
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(HURT);
    }

    private void OnDestroy()
    {
        if (_enemyAI != null)
            _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
        // enable attack collider for a short duration
        TriggerAttackAnimationTurnOn();
        CancelInvoke(nameof(TriggerAttackAnimationTurnOff));
        Invoke(nameof(TriggerAttackAnimationTurnOff), _attackColliderDuration);
    }
}