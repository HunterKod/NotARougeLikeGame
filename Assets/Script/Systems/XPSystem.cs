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

    public void AddXp(float amount)
    {
        currentXp += amount;
       
        while (currentXp >= xpToNextLevel)
        {
            currentXp -= xpToNextLevel;
            playerLevel++;
            Debug.Log($"XPSystem: Player leveled up! Level = {playerLevel}");

            xpToNextLevel *= xpGrowthPerLevel;
            EventManager.TriggerPlayerLevelUp();
        }
        EventManager.TriggerXpChanged(currentXp, xpToNextLevel);
    }

    private void OnEnable()
    {
        EventManager.OnEnemyKilledWithXp += AddXp;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyKilledWithXp -= AddXp;
    }
}
