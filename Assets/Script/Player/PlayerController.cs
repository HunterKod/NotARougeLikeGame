using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.18f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    public bool isRunning;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.1f, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        HandelMovement();

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandelMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0, vertical);

        if (inputDir.magnitude > 1)
        inputDir = inputDir.normalized;

        characterController.Move(inputDir * moveSpeed * Time.deltaTime);

        isRunning = inputDir != Vector3.zero;

        if (inputDir.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(inputDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
