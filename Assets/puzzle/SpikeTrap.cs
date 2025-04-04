using UnityEngine;
using System.Collections;
public class SpikeTrap : MonoBehaviour
{
    public float dropHeight = -1f;  // How far the spikes drop
    public float riseHeight = 0f;   // The normal height (starting position)
    public float speed = 2f;        // How fast the spikes move
    public float delayTime = 2f;    // Time before switching states

    private Vector3 loweredPosition;
    private Vector3 raisedPosition;
    private bool isRising = false;

    void Start()
    {
        raisedPosition = transform.position;  // Initial position
        loweredPosition = raisedPosition + new Vector3(0, dropHeight, 0);  
        StartCoroutine(SpikeRoutine());
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return MoveSpikes(loweredPosition);  // Drop spikes
            yield return new WaitForSeconds(delayTime);

            yield return MoveSpikes(raisedPosition);  // Raise spikes
            yield return new WaitForSeconds(delayTime);
        }
    }

    IEnumerator MoveSpikes(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
