using UnityEngine;

public class KentVisual : MonoBehaviour
{
    private Animator animator;

    [Header("Visual Components")]
    [SerializeField] private SpriteRenderer _bodyRenderer;
    [SerializeField] private SpriteRenderer _hairRenderer;
    [SerializeField] private SpriteRenderer _handRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string TRIGGER_ATTACK = "Attack";
    private const string HURT = "Hurt";


    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Знаходимо рендерери, якщо не призначені в інспекторі
        if (_bodyRenderer == null)
            _bodyRenderer = transform.Find("KentBodyVisual").GetComponent<SpriteRenderer>();
        if (_hairRenderer == null)
            _hairRenderer = transform.Find("KentHairVisual").GetComponent<SpriteRenderer>();
        if (_handRenderer == null)
            _handRenderer = transform.Find("KentHandVisual").GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        UpdateAnimation();
        UpdateFacingDirection();
    }

    private void UpdateAnimation()
    {
        animator.SetBool(IS_RUNNING, Kent.Instance.IsRunning());
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(TRIGGER_ATTACK);
    }

    /// <summary>
    /// Triggers hurt animation when Kent takes damage.
    /// </summary>
    public void PlayHurtAnimation()
    {
        animator.SetTrigger(HURT);
    }

    public void TriggerEndAttackAnimation()
    {
        Kent.Instance.EndAttack();
    }

    private void UpdateFacingDirection()
    {
        bool shouldFlip = !Kent.Instance.IsFacingRight();
        
        // Фліпаємо всі частини персонажа
        _bodyRenderer.flipX = shouldFlip;
        _hairRenderer.flipX = shouldFlip;
        _handRenderer.flipX = shouldFlip;
        // Фліпаємо колайдер атаки разом зі спрайтом
        Kent.Instance.UpdateAttackColliderDirection(Kent.Instance.IsFacingRight());
    }
}
