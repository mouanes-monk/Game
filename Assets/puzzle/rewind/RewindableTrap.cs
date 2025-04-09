using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpikeTrap))]
public class RewindableTrap : MonoBehaviour
{
    [Header("Rewind Settings")]
    public float maxRecordTime = 5f;
    public float rewindSpeed = 2f;
    public float dropHeight = 2f;

    private List<Vector3> positionHistory = new List<Vector3>();
    private bool isRewinding;
    private SpikeTrap spikeTrap;
    private Vector3 raisedPosition;
    private Vector3 loweredPosition;

    void Start()
    {
        spikeTrap = GetComponent<SpikeTrap>();
        raisedPosition = transform.position;
        loweredPosition = raisedPosition + new Vector3(0, -dropHeight, 0);
        StartCoroutine(SpikeRoutine());
    }

    void FixedUpdate()
    {
        if (isRewinding) return;

        positionHistory.Add(transform.position);

        while (positionHistory.Count > maxRecordTime / Time.fixedDeltaTime)
        {
            positionHistory.RemoveAt(0);
        }
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
        }

        if (spikeTrap != null)
            spikeTrap.enabled = true;

        isRewinding = false;
        positionHistory.Clear();
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