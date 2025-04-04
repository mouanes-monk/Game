using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpikeTrap))]
public class RewindableTrap : MonoBehaviour
{
    [Header("Rewind Settings")]
    public float maxRecordTime = 5f;
    public float rewindSpeed = 2f;

    [Header("Debug")]
    public bool showDebug = true;
    public bool showPath = true;

    private List<Vector3> positionHistory = new List<Vector3>();
    private bool isRewinding;
    private SpikeTrap spikeTrap;
    private Vector3 raisedPosition;
    private Vector3 loweredPosition;
    private float dropHeight = 2f;

    void Start()
    {
        spikeTrap = GetComponent<SpikeTrap>();
        raisedPosition = transform.position;
        loweredPosition = raisedPosition + new Vector3(0, -dropHeight, 0);
    }

    void FixedUpdate()
    {
        if (isRewinding) return;

        positionHistory.Add(transform.position);

        while (positionHistory.Count > maxRecordTime / Time.fixedDeltaTime)
        {
            positionHistory.RemoveAt(0);
        }

        if (showDebug && Time.frameCount % 30 == 0)
            Debug.Log($"[Trap] Recording Y={transform.position.y:F2}", this);
    }

    public void StartRewind()
    {
        if (!isRewinding && positionHistory.Count > 0)
        {
            if (spikeTrap != null)
                spikeTrap.enabled = false;

            StartCoroutine(RewindAnimation());
        }
    }

    IEnumerator RewindAnimation()
    {
        isRewinding = true;

        if (showDebug)
            Debug.Log($"[Trap] Starting rewind with {positionHistory.Count} frames", this);

        for (int i = positionHistory.Count - 1; i >= 0; i--)
        {
            float t = 0;
            Vector3 startPos = transform.position;

            while (t < 1f)
            {
                t += Time.deltaTime * rewindSpeed;
                transform.position = Vector3.Lerp(startPos, positionHistory[i], t);
                yield return null;
            }

            if (showDebug && i % 10 == 0)
                Debug.Log($"[Trap] Rewound to Y={positionHistory[i].y:F2}", this); // Fixed variable name here
        }

        if (spikeTrap != null)
            spikeTrap.enabled = true;

        isRewinding = false;
        positionHistory.Clear();

        if (showDebug)
            Debug.Log("[Trap] Rewind complete", this);
    }

    void OnDrawGizmos()
    {
        if (!showPath || positionHistory.Count < 2) return;

        Gizmos.color = Color.magenta;
        for (int i = 1; i < positionHistory.Count; i++)
        {
            Gizmos.DrawLine(positionHistory[i-1], positionHistory[i]);
        }
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return MoveToPosition(loweredPosition);
            yield return new WaitForSeconds(1f);
            yield return MoveToPosition(raisedPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        float t = 0;
        Vector3 start = transform.position;
        
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}