using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private TimeStop timeStop;
    [SerializeField] private Transform cameraTransform;

    public float baseMoveSpeed;
    public Vector3 MoveDirection { get; private set; }
    public bool CanRotate { get; set; } = true;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
=======
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
public float baseMoveSpeed;
    public Transform cameraTransform; // 🎯 Assign this in Inspector!
>>>>>>> parent of d2920d1 (.....)

    void Start()
    {baseMoveSpeed =moveSpeed;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
<<<<<<< HEAD
        rb.useGravity = true;
=======
>>>>>>> parent of d2920d1 (.....)
    }

    void Update()
    {
<<<<<<< HEAD
        if (GetComponent<PlayerRewind>()?.isRewinding == true) return;
=======
        if (GetComponent<PlayerRewind>().isRewinding) return;
>>>>>>> parent of d2920d1 (.....)

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        MoveDirection = (forward * moveZ + right * moveX).normalized;

        if (MoveDirection.magnitude > 0.1f)
        {
            animator.SetFloat("Speed", MoveSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

<<<<<<< HEAD
        if (CanRotate && MoveDirection.magnitude > 0.1f)
=======
        // 🔄 Smooth rotation toward movement
        if (canRotate && moveDirection.magnitude > 0.1f)
>>>>>>> parent of d2920d1 (.....)
        {
            Quaternion toRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            timeStop.FreezeEnemies(4.5f);
        }
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = MoveDirection * moveSpeed;
        rb.velocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
=======
        rb.velocity = moveDirection * moveSpeed + new Vector3(0, rb.velocity.y, 0);
>>>>>>> parent of d2920d1 (.....)
    }
}
