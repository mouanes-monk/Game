using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
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

    public Transform cameraTransform; // 🎯 Assign this in Inspector!

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (GetComponent<PlayerRewind>().isRewinding) return;

        float moveX = Input.GetAxisRaw("Horizontal"); // A/Q/D
        float moveZ = Input.GetAxisRaw("Vertical");   // W/Z/S

        // 🌍 Camera-relative movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        moveDirection = (forward * moveZ + right * moveX).normalized;

        // 🎵 Footstep + Animator
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

        // 🔄 Smooth rotation toward movement
        if (canRotate && moveDirection.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }
}
