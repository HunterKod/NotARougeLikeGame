using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) 
            return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {damageAmount} damage! Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");
        EventManager.TriggerPlayerDied();
    }

    private void Heal(float healAmount)
    {
        if (!isDead)
            return ;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player healed for {healAmount}! Current HP: {currentHealth}");
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    { 
        return maxHealth;
    }
}