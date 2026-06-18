using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Button[] upgradeButtons = new Button[3];

    private List<UpgradeOption> allPossibleUpgrades = new List<UpgradeOption>();

    private class UpgradeOption
    {
        public string Title;
        public string Description;
        public System.Action ApplyEffect;
    }

    private void Awake()
    {
        BuildUpgradePool();
    }

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLevelUp += ShowMenu;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerLevelUp -= ShowMenu;
    }

    private void BuildUpgradePool()
    {
        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "SwarmTactics", 
            Description = "+1 Homing Missile",
            ApplyEffect = () => playerCombat.UpgradeMissileCount(1)
        });

        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "Overcharge",
            Description = "+1 Lightning Target",
            ApplyEffect = () => playerCombat.UpgradeLightningStrike(1)
        });

        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "Centrifuge",
            Description = "Orbital Blades Spins 20% Faster",
            ApplyEffect = () => playerCombat.UpgradeOrbitalBlade(50f)
        });

        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "Brute Force",
            Description = "+15% Base Damage to All Attacks",
            ApplyEffect = () => playerCombat.globalDamageMultiplier += 0.15f
        });

        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "Swift Boots",
            Description = "Increase Movement Speed",
            ApplyEffect = () => playerStats.UpgradeMoveSpeed(1.5f)
        });

        allPossibleUpgrades.Add(new UpgradeOption
        {
            Title = "Vitality",
            Description = "+25 Max Health (Heals Instantly)",
            ApplyEffect = () => playerStats.UpgradeMaxHealth(25f)
        });
    }

    private void ShowMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;

        var randomUpgrades = allPossibleUpgrades.OrderBy(x => Random.value).Take(3).ToList();
        for (int i = 0; i < 3; i++)
        {
            var upgrade = randomUpgrades[i];
            Button btn = upgradeButtons[i];

            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = $"<b>{upgrade.Title}</b>\n<size=60%>{upgrade.Description}</size>";

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                upgrade.ApplyEffect();
                ResumeGame();
            });
        }
    }

    private void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
