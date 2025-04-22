using UnityEngine;

public class EmemyEntity : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int damageAmount)    
    {
        _currentHealth -= damageAmount;
        
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}