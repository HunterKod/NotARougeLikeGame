using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float healPercentage = 0.10f;
    [SerializeField] private float healChanceOnKill = 0.05f;

    private float currentHealth;
    private float currentMoveSpeed;
    private bool isDead = false;

    public float MoveSpeed => currentMoveSpeed;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentMoveSpeed = baseMoveSpeed;
    }

    private void Start()
    {
        EventManager.TriggerPlayerHealthChanged(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        EventManager.OnEnemyKilled += TryHealOnKill;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyKilled -= TryHealOnKill;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {damageAmount} damage! Current HP: {currentHealth}");

        EventManager.TriggerPlayerHealthChanged(currentHealth, maxHealth);

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
        if (isDead)
            return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        EventManager.TriggerPlayerHealthChanged(currentHealth, maxHealth);
    }

    private void TryHealOnKill()
    {
        if (isDead || currentHealth >= maxHealth)
            return;

        if (Random.value <= healChanceOnKill)
        {
            float healAmount = maxHealth * healPercentage;
            Heal(healAmount);
        }
    }

    public void UpgradeMaxHealth(float extraHealth)
    {
        maxHealth += extraHealth;
        currentHealth += extraHealth;
        EventManager.TriggerPlayerHealthChanged(currentHealth, maxHealth);
        Debug.Log($"Upgraded! Max health is now {maxHealth}");
    }

    public void UpgradeMoveSpeed(float extraSpeed)
    {
        currentMoveSpeed += extraSpeed;
        Debug.Log($"Upgraded! Movement Speed is now {currentMoveSpeed}");
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
