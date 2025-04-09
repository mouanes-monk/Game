using UnityEngine;

public class FloatingMage : MonoBehaviour
{
    public float floatHeight = 0.5f; // How high the mage floats up and down
    public float floatSpeed = 1f; // Speed of floating motion

    private Vector3 startPosition;
    private float floatOffset;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the floating motion using sine wave
        floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        
        // Apply the floating motion to the position
        transform.position = startPosition + new Vector3(0, floatOffset, 0);
    }
}