using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float doDamage = 10f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float xpValue = 10f;

    private float scaledMaxHealth;
    private float scaledDamage;
    private float scaledMoveSpeed;
    private float scaledXpValue;


    public float MoveSpeed => scaledMoveSpeed;
    public float GetDamage() => scaledDamage;
    public float GetXpValue() => scaledXpValue;

    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        ScaleStats(1f);
    }

    private void OnEnable()
    {
        EventManager.OnDifficultyChanged += ScaleStats;
    }

    private void OnDisable()
    {
        EventManager.OnDifficultyChanged -= ScaleStats;
    }

    public void TakeDamage(float takeDamage)
    {
        if (isDead)
            return;

        currentHealth -= takeDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, scaledMaxHealth);

        Debug.Log($"{gameObject.name} took {takeDamage} damage! HP: {currentHealth}/{scaledMaxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log($"{gameObject.name} died.");

        EventManager.TriggerEnemyKilledWithXp(scaledXpValue);
        EventManager.TriggerEnemyKilled();

        Destroy(gameObject);
    }

    public void ScaleStats(float multiplier)
    {
        scaledMaxHealth = maxHealth *= multiplier;
        scaledDamage = doDamage *= multiplier;
        scaledMoveSpeed = moveSpeed *= multiplier;
        scaledXpValue = xpValue *= multiplier;

        ResetHealth();

        Debug.Log($"EnemyStats: Applied multiplier {multiplier:F2} to {gameObject.name} -> HP={scaledMaxHealth}, Damage={scaledDamage}, Speed={scaledMoveSpeed}, XP={scaledXpValue}");
    }

    public void ResetHealth()
    {
        currentHealth = scaledMaxHealth;
        isDead = false;
    }
}
