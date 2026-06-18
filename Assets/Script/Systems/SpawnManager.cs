using UnityEngine;
using System.Collections;
using System.Threading;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private  GameObject enemyPrefab;
    [SerializeField] private float spwanRadius = 15f;
    [SerializeField] private float spwanInterval = 2f;
    [SerializeField] private int maxEnemies = 50;
    [SerializeField] private EnemyManager enemyManager;

    private int currentEnemies = 0;
    private bool spawning = true;
    private float difficultyMultiplier = 1f;
    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStartReceived;
        EventManager.OnEnemyKilled += OnEnemiesKilledReceived;
        EventManager.OnDifficultyChanged += OnDifficultyChangedReceived;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStartReceived;
        EventManager.OnEnemyKilled -= OnEnemiesKilledReceived;
        EventManager.OnDifficultyChanged -= OnDifficultyChangedReceived;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private void OnGameStartReceived(Transform playerTransform)
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        spawning = true;
        float timer = 0f;

        while (spawning) 
        {
            float currentInterval = Mathf.Max(0.1f, spwanInterval / Mathf.Sqrt(Mathf.Max(1f, difficultyMultiplier)));
            timer += Time.deltaTime;

            if (timer >= currentInterval)
            {
                if (currentEnemies < maxEnemies)
                {
                    SpawnEnemy();
                }
                timer = 0f;
            }
            yield return null;
        }
    }

    private void SpawnEnemy()
    {
        if (GameData.Instance?.PlayerTransform == null)
            return;

        Vector2 randomCircle = Random.insideUnitCircle * spwanRadius;

        Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);

        Vector3 spwanPos = GameData.Instance.PlayerTransform.position + randomOffset;
        spwanPos.y = 0;

        GameObject enemy = Instantiate(enemyPrefab, spwanPos, Quaternion.identity);
        currentEnemies++;

        if (enemyManager != null)
        {
            enemyManager.ApplyDifficultyToEnemy(enemy);
        }

        else
        {
            var manager = UnityEngine.Object.FindFirstObjectByType<EnemyManager>();
            if (manager != null)
            {
                manager.ApplyDifficultyToEnemy(enemy);
                Debug.LogWarning("SpawnManager: enemyManager was not assigned in inspector; used FindFirstObjectByType as fallback. Assign enemyManager in inspector to avoid this log.");
                enemyManager = manager;
            }

            else
            {
                Debug.LogWarning("SpawnManager: No EnemyManager found to apply difficulty! Make sure an EnemyManager exists and/or assign it in the SpawnManager inspector.");
            }
        }
    }

    private void OnDifficultyChangedReceived(float multiplier)
    {
        difficultyMultiplier = multiplier;
        Debug.Log($"SpawnManager: difficultyMultiplier updated to {difficultyMultiplier:F2}");
    }

    private void OnEnemiesKilledReceived()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }
}
