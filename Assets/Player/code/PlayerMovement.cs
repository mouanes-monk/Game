using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void Update()
    {
        if (GetComponent<PlayerRewind>()?.isRewinding == true) return;

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

        if (CanRotate && MoveDirection.magnitude > 0.1f)
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
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = MoveDirection * moveSpeed;
        rb.velocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
    }
}
