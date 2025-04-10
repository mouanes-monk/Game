using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Hold Positions")]
    public Transform holdPosition;
    public Transform buggyHoldPosition;

    [Header("Settings")]
    public float slowMovementSpeed = 2f;
    public float grabRange = 2.5f;
    public float followSpeed = 15f;
    public float holdDistance = 0.5f;

    [Header("Animation")]
    public Animator animator;

    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private PlayerMovement player;
    private bool isHolding = false;
    private bool isBuggyBox = false;
    private float normalMovementSpeed;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<PlayerMovement>();
        normalMovementSpeed = player.MoveSpeed;

        if (buggyHoldPosition == null)
        {
            buggyHoldPosition = holdPosition;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding) TryGrab();
            else DropObject();
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
                    player.MoveSpeed = slowMovementSpeed;
                    player.CanRotate = false;
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
            player.MoveSpeed = normalMovementSpeed;
            player.CanRotate = true;

            grabbedObject = null;
            grabbedRb = null;
            isBuggyBox = false;
        }
    }

    public bool IsHoldingObject()
    {
        return isHolding;
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
