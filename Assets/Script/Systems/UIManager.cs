using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    private int currentLevel = 1;

    private void OnEnable()
    {
        EventManager.OnPlayerHealthChanged += UpdateHealthUI;
        EventManager.OnXpChanged += UpdateXpUI;
        EventManager.OnPlayerLevelUp += UpdateLevelUI;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHealthChanged -= UpdateHealthUI;
        EventManager.OnXpChanged -= UpdateXpUI;
        EventManager.OnPlayerLevelUp -= UpdateLevelUI;
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.Ceil(currentHealth)} / {maxHealth}";
        }
    }

    private void UpdateXpUI(float currentXp, float xpToNextLevel)
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXp;
        }
    }

    private void UpdateLevelUI()
    {
        currentLevel++;
        if (levelText != null)
        {
            levelText.text = $"Lvl {currentLevel}";
        }
    }
}
