using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Hold Positions")]
    public Transform holdPosition;
    public Transform buggyHoldPosition;

    [Header("Settings")]
    public float slowMovementMultiplier = 0.4f;
    public float grabRange = 2.5f;
    public float followSpeed = 15f;
    public float holdDistance = 0.5f;

    // ✅ Fixed this line:
    public bool IsHolding => grabbedObject != null;

    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private PlayerMovement playerMovement;
    private bool isHolding = false;
    private bool isBuggyBox = false;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

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
                    grabbedRb.drag = 10f;
                    grabbedRb.angularDrag = 10f;

                    isBuggyBox = grabbedObject.name.ToLower().Contains("buggy");
                    isHolding = true;

                    playerMovement.grabMultiplier = slowMovementMultiplier;
                    playerMovement.canRotate = false;
                    playerMovement.UpdateMoveSpeed();
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

        Vector3 targetVelocity = direction * followSpeed;
        Vector3 force = (targetVelocity - grabbedRb.velocity) * grabbedRb.mass;

        grabbedRb.AddForce(force);

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
            grabbedRb.drag = 0f;
            grabbedRb.angularDrag = 0.05f;

            isHolding = false;

            playerMovement.grabMultiplier = 1f;
            playerMovement.canRotate = true;
            playerMovement.UpdateMoveSpeed();

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
