using System.Collections;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject[] rockGroups; // Array of rock group parent objects
    public float delayBeforeDisappearing = 1f; // Time before rocks disappear
    public float forceMagnitude = 10f; // Force magnitude to apply to each rock
    public float verticalForceRange = 5f; // Range for vertical force (up/down)

    void OnTriggerEnter(Collider other)
    {
        // Debugging: Log the collision to check when the ball hits something
        Debug.Log("Ball hit: " + other.gameObject.name);

        // Check if the ball hits a specific rock (tag or name can be used here)
        if (other.gameObject.CompareTag("BigRock"))
        {
            // Deactivate the BigRock upon impact
            other.gameObject.SetActive(false);
            Debug.Log("BigRock deactivated!");

           

            // Activate the rock group and apply random forces to rocks
            ActivateRockGroup();

            // Start coroutine to handle disappearing after a delay
            StartCoroutine(DeactivateRocksAfterDelay());
        }
    }

    void ActivateRockGroup()
    {
        // Activate all rock groups
        foreach (GameObject rockGroup in rockGroups)
        {
            rockGroup.SetActive(true); // Make rock group active

            // Apply random force to each rock in the group
            Rigidbody[] rockRigidbodies = rockGroup.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rockRigidbodies)
            {
                // Generate a random direction force for X and Z axes
                Vector3 randomForce = Random.insideUnitSphere * forceMagnitude;
                
                // Add vertical force (up and down)
                float verticalForce = Random.Range(-verticalForceRange, verticalForceRange);

                // Set the Y component of the force vector to the random vertical force
                randomForce.y = verticalForce;

                // Apply the random force to the rock's Rigidbody
                rb.AddForce(randomForce, ForceMode.Impulse);
                rb.useGravity = true; // Ensure gravity is applied for all rocks
            }
        }
    }

    IEnumerator DeactivateRocksAfterDelay()
    {
        // Wait for the specified delay before deactivating the rocks
        yield return new WaitForSeconds(delayBeforeDisappearing);

        // Deactivate the rock groups
        foreach (GameObject rockGroup in rockGroups)
        {
            rockGroup.SetActive(false); // Hide the rocks after the delay
        }
    }
}
