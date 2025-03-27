using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;   // Movement speed
    public Rigidbody rb;           // Reference to Rigidbody
    public Vector3 moveDirection;  // Store movement direction
    public float rotationSpeed = 10f; // Speed of rotation
  public float rayDistance = 2f; // Distance of the ray
  public bool canRotate = true; // Add this variable

    public LayerMask detectionLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents unwanted rotation
    }

    void Update()
    {
         // Define the ray origin (player position)
        Vector3 rayOrigin = transform.position;

        // Define the ray direction (forward)
        Vector3 rayDirection = transform.forward;

        // Cast the ray
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance, detectionLayer))
        {
            Debug.Log("Hit: " + hit.collider.name); // Show what we hit
        }

        // Draw the ray in the Scene view (for debugging)
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        // Get Input (WASD / Arrow Keys)
        float moveX = Input.GetAxisRaw("Horizontal"); // Left/Right
        float moveZ = Input.GetAxisRaw("Vertical");   // Up/Down

        // Set movement direction
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // Rotate player to face movement direction
        if (canRotate && moveDirection != Vector3.zero)  // Only rotate if moving
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = moveDirection * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }
}
