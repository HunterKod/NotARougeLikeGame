using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombat : MonoBehaviour
{
    public float globalDamageMultiplier = 1f;

    [SerializeField] private float damageBoostPerLevel = 0.05f;
    [SerializeField] private GameObject lightningStrikePrefab;
    [SerializeField] private float lightningStrikeCooldown = 2.0f;
    [SerializeField] private float lightningStrikeRange = 10f;
    [SerializeField] private int lightningStrikeCount = 3;

    [SerializeField] private GameObject orbitalBladePrefab;
    [SerializeField] private int orbitalBladeCount = 2;
    [SerializeField] private float orbitalBladeRadius = 3f;
    [SerializeField] private float orbitalBladeHeight = 2f;
    [SerializeField] private float orbitalBladeRotationSpeed = 180f;

    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float missileCooldown = 1.5f;
    [SerializeField] private int missileCount = 1;


    private float lightningStrikeTimer;
    private float missileTimer;
    private GameObject pivot;

    private List<GameObject> activeOrbitalBlades = new List<GameObject>();

    private void Start()
    {
        PerformOrbitalBlade();
    }

    private void Update()
    {
        lightningStrikeTimer -= Time.deltaTime;
        if (lightningStrikeTimer <= 0f)
        {
            PerformLightningStrike();
        }

        if (pivot != null)
        {
            pivot.transform.position = transform.position;
            pivot.transform.Rotate(Vector3.up, orbitalBladeRotationSpeed * Time.deltaTime);
        }

        missileTimer -= Time.deltaTime;
        if (missileTimer <= 0f)
        {
            PerformMissileAttack();
        }
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLevelUp += ApplyLevelUpScaling;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerLevelUp -= ApplyLevelUpScaling;
    }

    private void ApplyLevelUpScaling()
    {
        globalDamageMultiplier += damageBoostPerLevel;
        Debug.Log($"Player leveled up! Base Damage is now {globalDamageMultiplier * 100}%");

        foreach (GameObject blade in activeOrbitalBlades)
        {
            if (blade != null && blade.TryGetComponent<OrbitalBlade>(out var script))
            {
                script.SetDamageMultiplier(globalDamageMultiplier);
            }
        }
    }

    private void PerformLightningStrike()
    {
        if (lightningStrikePrefab == null)
            return;

        List<Transform> targets = GetClosestEnemies(lightningStrikeCount);

        if (targets.Count > 0)
        {
            for (int i = 0; i < lightningStrikeCount; i++)
            {
                Transform target = targets[i % targets.Count];

                GameObject newLightningStrike = Instantiate(lightningStrikePrefab, target.position, Quaternion.identity);

                if (newLightningStrike.TryGetComponent<LightningStrike>(out var script))
                {
                    script.SetDamageMultiplier(globalDamageMultiplier);
                }
            }

            EventManager.TriggerAbilityUsed();

            lightningStrikeTimer = lightningStrikeCooldown;
        }
    }

    private void PerformOrbitalBlade()
    {
        if (pivot != null)
        {
            Destroy(pivot);
            activeOrbitalBlades.Clear();
        }

        pivot = new GameObject("OrbitalBladePivot");
        
        float orbitalBladeAngle = 360f / orbitalBladeCount;

        for (int i = 0; i < orbitalBladeCount; i++)
        {
            float orbitalBladeCurrentAngle = i * orbitalBladeAngle;

            GameObject newOrbitalBlade = Instantiate(orbitalBladePrefab, pivot.transform);

            if (newOrbitalBlade.TryGetComponent<OrbitalBlade>(out var script))
            {
                script.SetDamageMultiplier(globalDamageMultiplier);
            }

            float xPos = Mathf.Cos(orbitalBladeCurrentAngle * Mathf.Deg2Rad) * orbitalBladeRadius;
            float zPos = Mathf.Sin(orbitalBladeCurrentAngle * Mathf.Deg2Rad) * orbitalBladeRadius;

            newOrbitalBlade.transform.localPosition = new Vector3(xPos, orbitalBladeHeight, zPos);
            newOrbitalBlade.transform.localRotation = Quaternion.Euler(0, orbitalBladeCurrentAngle, 0);

            activeOrbitalBlades.Add(newOrbitalBlade);
        }
    }

    private List<Transform> GetClosestEnemies(int maxTargets)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lightningStrikeRange);
        List<Transform> enemiesInRange = new List<Transform>();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyStats>(out _))
            {
                enemiesInRange.Add(hit.transform);
            }
        }

        return enemiesInRange
        .OrderBy(enemy => (enemy.position - transform.position).sqrMagnitude)
        .Take(maxTargets)
        .ToList();
    }

    private void PerformMissileAttack()
    {
        if (missilePrefab == null)
            return;

        for (int i = 0; i < missileCount; i++)
        {
            Vector3 missileSpawnOffset = new Vector3(Random.Range(-0.5f , 0.5f), 1f, Random.Range(-0.5f , 0.5f));
            GameObject newMissile = Instantiate(missilePrefab, transform.position + missileSpawnOffset, Quaternion.identity);

            if (newMissile.TryGetComponent<Missile>(out var script))
            {
                script.SetDamageMultiplier(globalDamageMultiplier);
            }
        }

        EventManager.TriggerAbilityUsed();
        missileTimer = missileCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, lightningStrikeRange);
    }

    public void UpgradeMissileCount(int amount)
    {
        missileCount += amount;
        Debug.Log($"Upgraded! Now firing {missileCount} missile.");
    }

    public void UpgradeLightningStrike(int amount)
    {
        lightningStrikeCount += amount;
        Debug.Log($"Upgraded! Now firing {lightningStrikeCount} lightning strike.");
    }

    public void UpgradeOrbitalBlade(float speed)
    {
        orbitalBladeRotationSpeed += speed;
        Debug.Log($"Upgraded! OrbitalBlade's speed is now {orbitalBladeRotationSpeed}");
    }
}
