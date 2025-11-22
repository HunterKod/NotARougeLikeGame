using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public float health = 100f;
    public float attack = 10f;
    public float defense = 5f;

    public void Initialize()
    {
        Debug.Log("PlayerStatsManager initialized");
    }
}
