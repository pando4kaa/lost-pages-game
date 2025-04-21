using UnityEngine;

public class KentVisual : MonoBehaviour
{
    private Animator animator;

    [Header("Visual Components")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer handRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string TRIGGER_ATTACK = "Attack";


    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Знаходимо рендерери, якщо не призначені в інспекторі
        if (bodyRenderer == null)
            bodyRenderer = transform.Find("KentBodyVisual").GetComponent<SpriteRenderer>();
        if (hairRenderer == null)
            hairRenderer = transform.Find("KentHairVisual").GetComponent<SpriteRenderer>();
        if (handRenderer == null)
            handRenderer = transform.Find("KentHandVisual").GetComponent<SpriteRenderer>();
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

    private void UpdateFacingDirection()
    {
        bool shouldFlip = !Kent.Instance.IsFacingRight();
        
        // Фліпаємо всі частини персонажа
        bodyRenderer.flipX = shouldFlip;
        hairRenderer.flipX = shouldFlip;
        handRenderer.flipX = shouldFlip;
    }
}
