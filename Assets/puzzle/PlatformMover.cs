using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public float moveDistance = 5f;     // Total distance to move left and right
    public float moveSpeed = 2f;        // Speed of movement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // PingPong returns a value that goes back and forth between 0 and moveDistance
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPos + Vector3.right * (offset - moveDistance / 2f);
    }
}
