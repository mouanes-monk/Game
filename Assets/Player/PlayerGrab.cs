using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public Transform holdPosition; // Position where the object will be held
    public float slowMovementSpeed = 2f; // Slower movement when holding
    private float normalMovementSpeed; // Store original speed

    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Reference to movement script
        normalMovementSpeed = playerMovement.moveSpeed; // Save normal speed
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press E to grab/drop
        {
            if (grabbedObject == null)
                TryGrab();
            else
                DropObject();
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f)) // Cast a ray in front
        {
            if (hit.collider.CompareTag("box")) // Only grab objects with this tag
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRb = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRb)
                {
                    grabbedRb.isKinematic = true; // Disable physics while holding
                    grabbedObject.transform.SetParent(holdPosition);
                    grabbedObject.transform.localPosition = Vector3.zero;
                    grabbedObject.transform.localRotation = Quaternion.identity;

                    // Slow movement and disable rotation
                    playerMovement.moveSpeed = slowMovementSpeed;
                    playerMovement.canRotate = false; // Disable rotation
                }
            }
        }
    }

    void DropObject()
    {
        if (grabbedObject)
        {
            grabbedRb.isKinematic = false; // Enable physics again
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;

            // Restore normal movement and rotation
            playerMovement.moveSpeed = normalMovementSpeed;
            playerMovement.canRotate = true; // Enable rotation again
        }
    }
}
