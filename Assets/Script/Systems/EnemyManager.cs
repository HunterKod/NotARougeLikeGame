using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //[SerializeField] private float basePoolSize = 10;

    private float currentMultiplier = 1f;

    private void OnEnable()
    {
        EventManager.OnDifficultyChanged += OnDifficultyChangedReceived;
    }

    private void OnDisable()
    {
        EventManager.OnDifficultyChanged -= OnDifficultyChangedReceived;
    }

    private void OnDifficultyChangedReceived(float multiplier)
    {
        currentMultiplier = multiplier;
        Debug.Log($"EnemyManager: Difficulty changed -> {currentMultiplier:F2}");
    }

    public void ApplyDifficultyToEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

        var stats = enemy.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.ScaleStats(currentMultiplier);
            stats.ResetHealth();
        }
    }
}
