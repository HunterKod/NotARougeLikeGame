using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private float stoppingDistance = 1.2f;
    [SerializeField] private float minMoveDistance = 0.05f;


    private Rigidbody rb;
    private EnemyStats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
    }

    private void AssignPlayer(Transform playerTransform)
    {
        targetPlayer = playerTransform;
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

    private void FixedUpdate()
    {
        if (targetPlayer == null)
            return;

        Vector3 toPlayer = targetPlayer.position - transform.position;
        Vector3 toPlayerFlat = new Vector3(toPlayer.x, 0f, toPlayer.z);
        float sqrDistance = toPlayerFlat.sqrMagnitude;

        float stopDist = stoppingDistance;
        if (sqrDistance > stopDist)
        {
            Vector3 moveDir = toPlayerFlat.normalized;

            float desiredMove = stats.MoveSpeed * Time.fixedDeltaTime;
            float allowedDistanceToMove = sqrDistance - stopDist;
            float actualMove = Mathf.Min(desiredMove, allowedDistanceToMove);

            if (actualMove > minMoveDistance)
            {
                Vector3 movement = moveDir * actualMove;
                rb.MovePosition(rb.position + movement);
            }
            
        }

        else
        {
            rb.linearVelocity = Vector3.zero;
        }

        Vector3 lookDir = toPlayerFlat;
        if (lookDir != Vector3.zero)
        {
            float rotationSpeed = 10f;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}

