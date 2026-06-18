using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] private float lightningStrikeDamage = 50f;
    [SerializeField] private float strikeRadius = 2f;
    [SerializeField] private float visualTime = 0.2f;

    private float damageMultiplier = 1f;

    private void Start()
    {
        ApplyDamage();

        Destroy(gameObject, visualTime);
    }

    public void SetDamageMultiplier(float newMultiplier)
    {
        damageMultiplier = newMultiplier;
    }

    private void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, strikeRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<EnemyStats>(out var enemy))
            {
                float totalDamage = lightningStrikeDamage * damageMultiplier;
                enemy.TakeDamage(totalDamage);
                Debug.Log($"Lightning Strike hit {hitCollider.name} for {totalDamage} damage!");
            }
        }
    }
}
