using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Hold Positions")]
    public Transform holdPosition;          
    public Transform buggyHoldPosition;    

    [Header("Settings")]
    public float slowMovementSpeed = 2f;    
    public float grabRange = 2.5f;          
    public float followSpeed = 15f;         // Increased for better response
    public float holdDistance = 0.5f;       // Added hold distance parameter

    private GameObject grabbedObject;      
    private Rigidbody grabbedRb;           
    private PlayerMovement playerMovement; 
    private float normalMovementSpeed;     
    private bool isHolding = false;        
    private bool isBuggyBox = false;

    [Header("Animation")]
    public Animator animator;              

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        normalMovementSpeed = playerMovement.moveSpeed;

        if (buggyHoldPosition == null)
        {
            buggyHoldPosition = holdPosition;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding)
                TryGrab();
            else
                DropObject();
        }

        if (isHolding && grabbedObject != null)
        {
            MoveHeldObject();
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f;
        Vector3 rayDirection = transform.forward;

        if (Physics.SphereCast(rayOrigin, 0.5f, rayDirection, out hit, grabRange))
        {
            if (hit.collider.CompareTag("box"))
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRb = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRb)
                {
                    animator.SetBool("Grab", true);
                    grabbedRb.useGravity = false;
                    grabbedRb.drag = 10f;              // Added drag for stability
                    grabbedRb.angularDrag = 10f;       // Added angular drag

                    isBuggyBox = grabbedObject.name.ToLower().Contains("buggy");
                    isHolding = true;
                    playerMovement.moveSpeed = slowMovementSpeed;
                    playerMovement.canRotate = false;
                }
            }
        }
    }

    void MoveHeldObject()
    {
        if (grabbedRb == null) return;

        Transform targetHold = isBuggyBox ? buggyHoldPosition : holdPosition;
        Vector3 targetPosition = targetHold.position;
        Vector3 direction = targetPosition - grabbedRb.position;

        // Calculate desired velocity
        Vector3 targetVelocity = direction * followSpeed;
        
        // Calculate force needed
        Vector3 force = (targetVelocity - grabbedRb.velocity) * grabbedRb.mass;
        
        // Apply force while maintaining physics
        grabbedRb.AddForce(force);
        
        // Maintain distance
        if (direction.magnitude > holdDistance)
        {
            grabbedRb.velocity = direction.normalized * followSpeed;
        }
        else
        {
            grabbedRb.velocity = Vector3.zero;
        }
    }

    void DropObject()
    {
        if (grabbedObject)
        {
            animator.SetBool("Grab", false);
            grabbedRb.useGravity = true;
            grabbedRb.drag = 0f;           // Reset drag
            grabbedRb.angularDrag = 0.05f; // Reset angular drag

            isHolding = false;
            playerMovement.moveSpeed = normalMovementSpeed;
            playerMovement.canRotate = true;

            grabbedObject = null;
            grabbedRb = null;
            isBuggyBox = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f;
        Vector3 rayEnd = rayOrigin + transform.forward * grabRange;
        Gizmos.DrawLine(rayOrigin, rayEnd);
        Gizmos.DrawWireSphere(rayEnd, 0.5f);
    }
}