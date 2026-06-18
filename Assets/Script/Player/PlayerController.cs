using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    [SerializeField]private PlayerStats playerStats;
    private Vector3 verticalVelocity;
    private bool isGrounded;
    public bool isRunning;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        ApplyGravity();
        HandelMovement();
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        else
        {
            verticalVelocity.y += gravity * Time.deltaTime; 
        }

        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void HandelMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            isRunning = true;

            characterController.Move(inputDir * playerStats.MoveSpeed * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(inputDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        else
        {
            isRunning = false;
        }
    }

    public bool IsRunning => isRunning;
}
