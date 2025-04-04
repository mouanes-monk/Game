using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform plate; // Assign the plate object
    public Transform palmTree; // Assign the palm tree object

    public Vector3 pressedPositionOffset = new Vector3(0, -0.2f, 0); // How much the plate moves down
    private Vector3 defaultPosition; // The starting position of the plate

    public Vector3 rewindRotation = new Vector3(-15f, 0f, 0f); // Palm tree tilting
    private Quaternion originalPalmRotation; // Store default palm rotation

    public float moveSpeed = 5f; // How fast the plate moves
    public float rewindSpeed = 3f; // How fast the palm moves

    private int objectsOnPlate = 0; // Count how many objects are on the plate

    void Start()
    {
        defaultPosition = plate.localPosition; // Store the original position
        originalPalmRotation = palmTree.localRotation; // Store the original rotation
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("box"))
        {
            objectsOnPlate++; // Count how many objects are on the plate
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("box"))
        {
            objectsOnPlate = Mathf.Max(0, objectsOnPlate - 1); // Prevent negative values
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("box"))
        {
            objectsOnPlate = 1; // Ensure the plate stays down
        }
    }

    void Update()
    {
        bool isActivated = objectsOnPlate > 0;

        // Move the plate up/down smoothly
        Vector3 targetPosition = isActivated ? defaultPosition + pressedPositionOffset : defaultPosition;
        plate.localPosition = Vector3.Lerp(plate.localPosition, targetPosition, Time.deltaTime * moveSpeed);

        // Rotate the palm tree smoothly
        Quaternion targetRotation = isActivated ? Quaternion.Euler(rewindRotation) : originalPalmRotation;
        palmTree.localRotation = Quaternion.Lerp(palmTree.localRotation, targetRotation, Time.deltaTime * rewindSpeed);
    }
}
