using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float baseMoveSpeed;
    public float grabMultiplier = 1f;
    public float zoneMultiplier = 1f;

    public Rigidbody rb;
    public Vector3 moveDirection;
    public float rotationSpeed = 10f;
    public bool canRotate = true;
    public Animator animator;
    public LayerMask detectionLayer;
    public AudioSource footstepSource;
    public AudioClip footstepSound;
    public float stepInterval = 0.4f;
    private float stepTimer = 0f;
    public Transform cameraTransform;

    [Header("Gravity Settings")]
    public float gravityForce = 9.8f;
    private bool isGrounded;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.useGravity = true;

        UpdateMoveSpeed(); // Set initial calculated speed
    }

    void Update()
    {
        if (GetComponent<PlayerRewind>().isRewinding) return;

        // Check ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * moveZ + right * moveX).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = footstepSound;
                footstepSource.loop = true;
                footstepSource.Play();
            }
            animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            footstepSource.Stop();
            animator.SetFloat("Speed", 0);
        }

        if (canRotate && moveDirection.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        if (!isGrounded)
        {
            currentVelocity.y -= gravityForce * Time.fixedDeltaTime;
        }
        else if (currentVelocity.y < 0)
        {
            currentVelocity.y = 0;
        }

        rb.velocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
    }

    public void UpdateMoveSpeed()
    {
        moveSpeed = baseMoveSpeed * grabMultiplier * zoneMultiplier;

        if (animator != null)
            animator.speed = zoneMultiplier; // Optional: animation speed is only affected by zone
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
