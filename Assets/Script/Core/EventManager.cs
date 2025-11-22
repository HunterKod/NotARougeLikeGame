using System;
using UnityEngine;

public class EventManager
{
    public static Action <Transform> OnGameStart;
    public static Action OnEnemyKilled;
    public static Action <float> OnEnemyKilledWithXp;
    public static Action OnPlayerLevelUp;
    public static Action <float> OnDifficultyChanged;
    public static Action <float, float> OnXpChanged;
    public static Action OnPlayerDied;
    public static Action OnItemCollected;
    public static Action OnAbilityUsed;
    public static Action OnPlayerInteract;

    public static void TriggerGameStart(Transform playerTransform)
    {
        OnGameStart?.Invoke(playerTransform);
    }

    public static void TriggerEnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

    public static void TriggerEnemyKilledWithXp(float xp)
    {
        OnEnemyKilledWithXp?.Invoke(xp);
    }

    public static void TriggerPlayerLevelUp()
    {
        OnPlayerLevelUp?.Invoke();
    }

    public static void TriggerDifficultyChanged(float multiplier) 
    {
        OnDifficultyChanged?.Invoke(multiplier);
    }

    public static void TriggerXpChanged(float currentXp, float xpToNext)
    {
        OnXpChanged?.Invoke(currentXp, xpToNext);
    }

    public static void TriggerPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public static void TriggerItemCollected()
    {
        OnItemCollected?.Invoke();
    }

    public static void TriggerAbilityUsed()
    {
        OnAbilityUsed?.Invoke();
    }

    public static void TriggerPlayerInteract()
    {
        OnPlayerInteract?.Invoke();
    }
}
