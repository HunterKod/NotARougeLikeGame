using UnityEngine;
using UnityEngine.Rendering;

public class LowHealthEffect : MonoBehaviour
{
    [SerializeField] private Volume lowHealthVolume;
    [SerializeField] private float healthThreshold = 0.5f;
    [SerializeField] private float minPulseSpeed = 3f;
    [SerializeField] private float maxPulseSpeed = 12f;
    [SerializeField] private float pulseAmplititude = 0.2f;

    private float targetBaseWeight = 0f;
    private float currentBaseWeight = 0f;

    private void Start()
    {
        if (lowHealthVolume != null)
        {
            lowHealthVolume.weight = 0f;
        }
    }

    private void OnEnable()
    {
        EventManager.OnPlayerHealthChanged += UpdateHealthEffect;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerHealthChanged -= UpdateHealthEffect;
    }

    private void Update()
    {
        if (lowHealthVolume == null)
            return;

        currentBaseWeight = Mathf.Lerp(currentBaseWeight, targetBaseWeight, Time.deltaTime * 3f);

        if (targetBaseWeight > 0)
        {
            float currentPulseSpeed = Mathf.Lerp(minPulseSpeed, maxPulseSpeed, targetBaseWeight);
            
            float normalizedSin = (Mathf.Sin(Time.time * currentPulseSpeed) + 1f) / 2f;

            float finalWeight = currentBaseWeight + (normalizedSin * pulseAmplititude);

            lowHealthVolume.weight = Mathf.Clamp01(finalWeight);
        }

        else
        {
            lowHealthVolume.weight = currentBaseWeight;
        }
    }

    private void UpdateHealthEffect(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage <= healthThreshold)
        {
            targetBaseWeight = 1f - (healthPercentage / healthThreshold);
        }

        else
        {
            targetBaseWeight = 0f;
        }

        Debug.Log($"[Health Math] HP: {currentHealth}/{maxHealth} | Percent: {healthPercentage} | Threshold: {healthThreshold} | Target Weight: {targetBaseWeight}");
    }
}
