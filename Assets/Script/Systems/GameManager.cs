using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        Invoke(nameof(StartGame), 0.5f);
    }

    private void StartGame()
    {
        GameData.Instance.SetPlayer(playerTransform);

        EventManager.TriggerGameStart(playerTransform);
    }
}
