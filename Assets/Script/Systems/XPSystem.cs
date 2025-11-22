using UnityEngine;

public class XPSystem : MonoBehaviour
{
    public int playerLevel = 1;
    public float currentXp = 0f;
    public float xpToNextLevel = 100f;
    public float xpPerKill = 10f;
    public float xpGrowthPerLevel = 1.25f;

    private void Start()
    {
        EventManager.TriggerXpChanged(currentXp, xpToNextLevel);
    }

    private void AddXp()
    {
        AddXp(xpPerKill);
    }

    public void AddXp(float amount)
    {
        currentXp += amount;
        Debug.Log($"XPSystem: Gained {amount} XP. currentXP = {currentXp}/{xpToNextLevel}");
        EventManager.TriggerXpChanged(currentXp, xpToNextLevel);

        while (currentXp >= xpToNextLevel)
        {
            playerLevel++;
            Debug.Log($"XPSystem: Player leveled up! Level = {playerLevel}");
            xpToNextLevel *= xpGrowthPerLevel;
            EventManager.TriggerXpChanged(currentXp, xpToNextLevel);
            EventManager.TriggerPlayerLevelUp();
        }
    }

    private void OnEnable()
    {
        EventManager.OnEnemyKilled += AddXp;
        EventManager.OnEnemyKilledWithXp += AddXp;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyKilled -= AddXp;
        EventManager.OnEnemyKilledWithXp -= AddXp;
    }
}
