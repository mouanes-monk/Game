using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool canRotate = true;
    [SerializeField] private Animator animator;
    [SerializeField] private TimeStop timeStop;

    [SerializeField] private LayerMask detectionLayer;

    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepSound;

    [SerializeField] private float stepInterval = 0.4f;
    private float stepTimer = 0f;
<<<<<<< Updated upstream
public float baseMoveSpeed;
    public Transform cameraTransform; // 🎯 Assign this in Inspector!
=======

    [SerializeField] private Transform cameraTransform; // 🎯 Assign this in Inspector!

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }

    public Vector3 MoveDirection
    {
        get => moveDirection;
        set => moveDirection = value;
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    public bool CanRotate
    {
        get => canRotate;
        set => canRotate = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public LayerMask DetectionLayer
    {
        get => detectionLayer;
        set => detectionLayer = value;
    }

    public AudioSource FootstepSource
    {
        get => footstepSource;
        set => footstepSource = value;
    }

    public AudioClip FootstepSound
    {
        get => footstepSound;
        set => footstepSound = value;
    }

    public float StepInterval
    {
        get => stepInterval;
        set => stepInterval = value;
    }

    public float StepTimer
    {
        get => stepTimer;
        set => stepTimer = value;
    }

    public Transform CameraTransform
    {
        get => cameraTransform;
        set => cameraTransform = value;
    }

>>>>>>> Stashed changes

    void Start()
    {baseMoveSpeed =moveSpeed;
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            timeStop.FreezeEnemies(4.5f);
        }

        // 🎵 Footstep + Animator
        if (moveDirection.magnitude > 0.1f)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = footstepSound;
                footstepSource.loop = true;
                footstepSource.Play();
            }
            animator.SetFloat("Speed", MoveSpeed);
        }
        else
        {
            footstepSource.Stop();
            animator.SetFloat("Speed", 0);
        }

        // 🔄 Smooth rotation toward movement
        if (CanRotate && moveDirection.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * MoveSpeed + new Vector3(0, rb.velocity.y, 0);
    }
}
