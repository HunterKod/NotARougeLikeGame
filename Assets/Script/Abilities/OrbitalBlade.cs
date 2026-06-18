using UnityEngine;
using System.Collections.Generic;

public class OrbitalBlade : MonoBehaviour
{
    [SerializeField] private float orbitalBladeDamage = 50f;

    private float damageMultiplier = 1f;

    public void SetDamageMultiplier(float newMultiplier)
    {
        damageMultiplier = newMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyStats>(out var enemy))
        {
            float totalDamage = orbitalBladeDamage * damageMultiplier;
            enemy.TakeDamage(totalDamage);
            Debug.Log($"Orbital Blade sliced {other.name} for {totalDamage} damage!");
        }
    }
}
