using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public Transform holdPosition; // Where the object is held
    public float slowMovementSpeed = 2f;
    private float normalMovementSpeed;
    private bool isHolding = false;

    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private PlayerMovement playerMovement;
    public Animator animator;
    public  float grabRange = 2.5f;

    void Start()
    {  
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        normalMovementSpeed = playerMovement.moveSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press E to grab/drop
        {
            if (!isHolding)
                TryGrab();
            else
                DropObject();
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        // **Increased range**
        float sphereRadius = 0.5f; // **Wider detection**

        // **Fix Raycast Position: Start at chest height, not feet**
        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f; // Chest-level cast
        Vector3 rayDirection = transform.forward;

        // **Use SphereCast for better object detection**
        if (Physics.SphereCast(rayOrigin, sphereRadius, rayDirection, out hit, grabRange))
        {
            if (hit.collider.CompareTag("box")) 
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRb = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRb)
                {   
                    animator.SetBool("Grab", true);
                    grabbedRb.isKinematic = true;

                    // **Align object with hands**
                    grabbedObject.transform.SetParent(holdPosition, true);
                    grabbedObject.transform.position = holdPosition.position;
                    grabbedObject.transform.rotation = holdPosition.rotation;

                    isHolding = true;
                    playerMovement.moveSpeed = slowMovementSpeed;
                    playerMovement.canRotate = false;
                }
            }
        }
    }

    void DropObject()
    {
        if (grabbedObject)
        {
            grabbedObject.transform.SetParent(null, true);

            grabbedRb.isKinematic = false; 
            grabbedRb.WakeUp(); 
            
            Physics.SyncTransforms(); 

            animator.SetBool("Grab", false);
            isHolding = false;
            playerMovement.moveSpeed = normalMovementSpeed;
            playerMovement.canRotate = true;

            grabbedObject = null;
            grabbedRb = null;
        }
    }

    // **Visualize Corrected Raycast in Scene View**
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f; // **Chest-level ray**
        Vector3 rayEnd = rayOrigin + transform.forward * 2.5f;

        Gizmos.DrawLine(rayOrigin, rayEnd);
        Gizmos.DrawWireSphere(rayEnd, 0.5f); // **Shows detection area**
    }
}
