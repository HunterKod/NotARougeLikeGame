using UnityEngine;

public class SwordAttackVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (slashEffect != null)
            {
                slashEffect.Play();
            }
        }
    }
}
