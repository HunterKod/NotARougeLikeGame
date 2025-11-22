using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private EnemyStats enemyStats;
    private float lastAttackTime;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        if (GameData.Instance?.PlayerTransform != null)
        {
            player = GameData.Instance.PlayerTransform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        lastAttackTime = Time.time;

        PlayerStats playerStats = player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(enemyStats.GetDamage());
        }

        Debug.Log($"{gameObject.name} attacked player for {enemyStats.GetDamage()} damage!");

    }
}
