using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    //[SerializeField] private int _maxHealth;
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private bool _isDead = false;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[EnemyEntity] OnTriggerEnter2D: {collision.gameObject.name}");
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        Debug.Log($"[EnemyEntity] TakeDamage called. Damage: {damage}, CurrentHealth before: {_currentHealth}, Enemy: {gameObject.name}");
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        Debug.Log($"[EnemyEntity] DetectDeath called. CurrentHealth: {_currentHealth}, Enemy: {gameObject.name}");
        if (_currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            Debug.Log($"[EnemyEntity] Enemy died: {gameObject.name}");
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;

            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }


}
