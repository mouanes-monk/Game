using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public Transform holdPosition; // Where the object is held
    public float slowMovementSpeed = 2f; // Movement speed when holding
    private float normalMovementSpeed;
    private bool isHolding = false;

    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        normalMovementSpeed = playerMovement.moveSpeed;
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("box")) 
            {
                grabbedObject = hit.collider.gameObject;
                grabbedRb = grabbedObject.GetComponent<Rigidbody>();

                if (grabbedRb)
                {
                    grabbedRb.isKinematic = true;
                    grabbedObject.transform.SetParent(holdPosition, true);
                    grabbedObject.transform.localPosition = Vector3.zero;
                    grabbedObject.transform.rotation = Quaternion.identity;

                    // **Disable rotation and slow movement**
                    isHolding = true;
                    playerMovement.moveSpeed = slowMovementSpeed;
                    playerMovement.canRotate = false; // Stop player from rotating
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

            // **Restore movement speed and rotation**
            isHolding = false;
            playerMovement.moveSpeed = normalMovementSpeed;
            playerMovement.canRotate = true;

            grabbedObject = null;
        }
    }
}
