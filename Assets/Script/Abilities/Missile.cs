using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float missileSpeed = 10f;
    [SerializeField] private float missileRotationSpeed = 5f;
    [SerializeField] private float missileDamage = 20f;
    [SerializeField] private float missileLifetime = 5f;
    [SerializeField] private float missileDetectionRadius = 15f;

    private float damageMultiplier = 1f;
    private Transform target;

    private void Start()
    {
        FindNewTarget();
        Destroy(gameObject, missileLifetime);
    }

    private void Update()
    {
        if (target == null)
        {
            FindNewTarget();
        }

        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, missileRotationSpeed * Time.deltaTime);
            }
        }

        transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
    }

    public void SetDamageMultiplier(float newMultiplier)
    {
        damageMultiplier = newMultiplier;
    }

    private void FindNewTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, missileDetectionRadius);
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyStats>(out _))
            {
                float distance = (hit.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = hit.transform;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyStats>(out var enemy))
        {
            float totalDamage = missileDamage * damageMultiplier;
            enemy.TakeDamage(totalDamage);
            Debug.Log($"{other.name} took {totalDamage} damage from Missile!");
            Destroy(gameObject);
        }
    }
}
