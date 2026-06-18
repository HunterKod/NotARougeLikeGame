using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private float stoppingDistance = 1.0f;
    [SerializeField] private float acceleration = 5.0f;

    private Rigidbody rb;
    private EnemyStats stats;
    private float currentSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
    }  

    private void OnEnable()
    {
        EventManager.OnGameStart += AssignPlayer;

        if (GameData.Instance?.PlayerTransform != null)
        {
            targetPlayer = GameData.Instance.PlayerTransform;
        }
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= AssignPlayer;
    }

    private void AssignPlayer(Transform playerTransform)
    {
        targetPlayer = playerTransform;
    }

    private void FixedUpdate()
    {
        if (targetPlayer == null)
            return;

        Vector3 toPlayer = targetPlayer.position - transform.position;
        Vector3 toPlayerFlat = new Vector3(toPlayer.x, 0f, toPlayer.z);
        float currentDistance = toPlayerFlat.magnitude;

        if (currentDistance > 0.05f)
        {
            Vector3 targetVelocity = Vector3.zero;

            if (currentDistance > stoppingDistance)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, acceleration * Time.fixedDeltaTime);
                Vector3 moveDir = toPlayerFlat.normalized;
                targetVelocity = moveDir * stats.MoveSpeed;
            }

            targetVelocity.y = rb.linearVelocity.y;

            rb.linearVelocity = targetVelocity;
            
            Quaternion targetRotation = Quaternion.LookRotation(toPlayerFlat);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }
}
