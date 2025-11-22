using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float lifetime = 0.2f;

    private Collider swordCollider;

    private void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // Destroy the sword swing after it finishes
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit an enemy
        if (other.TryGetComponent<EnemyStats>(out var enemyStats))
        {
            enemyStats.TakeDamage(damage);
            Debug.Log($"Sword hit {other.name} for {damage} damage!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider>();
        }

        if (swordCollider != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);

            if (swordCollider is SphereCollider sphere)
            {
                Gizmos.DrawSphere(transform.position + sphere.center, sphere.radius);
            }
            else if (swordCollider is BoxCollider box)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position + box.center, box.size);
            }

        }
    }
}
