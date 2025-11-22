using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    [SerializeField] private float difficultyMultiplier = 1f;
    [SerializeField] private float difficultyOverTime = 20f; //Increase the diff over time
    [SerializeField] private float difficultyOverTimeMultiplier = 0.05f; //Multiplier for the diff over time
    [SerializeField] private float difficultyOverLevel = 0.15f;
    [SerializeField] private float killRateWindow = 10f; //Seconds to track kills in that time so the diff scaler can change diff based on the kills done
    [SerializeField] private float killsForBoost = 5f; //Max kills to trigger the diff scaler to boost the diff more
    [SerializeField] private float killsForBoostAmount = 0.1f; //Diff scaler multiplier

    private readonly List<float> killTimeStamp = new List<float>();
    private Coroutine timeIncreaseCoroutine;

    private void Start()
    {
        EventManager.TriggerDifficultyChanged(difficultyMultiplier);
    }
    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStartReceived;
        EventManager.OnEnemyKilled += OnEnemyKilledReceived;
        EventManager.OnPlayerLevelUp += OnPlayerLevelUpReceived;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStartReceived;
        EventManager.OnEnemyKilled -= OnEnemyKilledReceived;
        EventManager.OnPlayerLevelUp -= OnPlayerLevelUpReceived;
        if (timeIncreaseCoroutine != null) StopCoroutine(timeIncreaseCoroutine);
    }

    private void OnGameStartReceived(Transform playerTransform)
    {
        difficultyMultiplier = 1f;
        killTimeStamp.Clear();

        if (timeIncreaseCoroutine != null) StopCoroutine(timeIncreaseCoroutine);
        timeIncreaseCoroutine = StartCoroutine(TimeBasedIncreaseRoutine());
        EventManager.TriggerDifficultyChanged(difficultyMultiplier);
    }

    private IEnumerator TimeBasedIncreaseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyOverTime);
            IncreaseDifficulty(difficultyOverTimeMultiplier);
        }
    }

    private void IncreaseDifficulty(float amount)
    {
        difficultyMultiplier += amount;
        Debug.Log($"DifficultyScaler: Increased by {amount}. New multiplier = {difficultyMultiplier:F2}");
        EventManager.TriggerDifficultyChanged(difficultyMultiplier);
    }

    private void OnEnemyKilledReceived()
    {
        killTimeStamp.Add(Time.time);
        float cutoff = Time.time - killRateWindow;
        killTimeStamp.RemoveAll(t => t < cutoff);

        if (killTimeStamp.Count >= killsForBoost)
        {
            Debug.Log($"DifficultyScaler: High kill-rate detected ({killTimeStamp.Count} kills in {killRateWindow}s). Applying boost.");
            IncreaseDifficulty(killsForBoostAmount);
            killTimeStamp.Clear();
        }
    }

    private void OnPlayerLevelUpReceived()
    {
        IncreaseDifficulty(difficultyOverLevel);
    }

    public float GetMultiplier()
    {
        return difficultyMultiplier;
    }
}
