using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;

    private void OnEnable()
    {
        EventManager.OnEnemyDamaged += SpwanDamageText;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDamaged -= SpwanDamageText;
    }

    private void SpwanDamageText(Vector3 position, float damageAmount)
    {
        if (damageTextPrefab == null)
            return;

        Vector3 spawnPos = position + (Vector3.up * 1.5f);
        GameObject textObj = Instantiate (damageTextPrefab, spawnPos, Quaternion.identity);

        if (textObj.TryGetComponent<DamageTextUI>(out var damageTextScript))
        {
            damageTextScript.Setup(damageAmount);
        }
    }
}
