using UnityEngine;

public class PlayerCombact : MonoBehaviour
{
    [SerializeField] private GameObject swordAttackPrefab;
    [SerializeField] private Transform attackSpawnPoint;
    [SerializeField] private float attackCooldown = 1.0f;

    private float attackTimer;

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            PerformAttack();
            attackTimer = attackCooldown;
        }
    }

    private void PerformAttack()
    {
        if (swordAttackPrefab != null && attackSpawnPoint != null)
        {
            GameObject attack = Instantiate(swordAttackPrefab, attackSpawnPoint.position, attackSpawnPoint.rotation * Quaternion.Euler(-90f, 0f, 130f));

            EventManager.TriggerAbilityUsed();

            Debug.Log("AttackTriggered");
        }
    }
}
