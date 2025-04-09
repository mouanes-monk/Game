using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRewind : MonoBehaviour
{
    [Header("Core Settings")]
    public float recordTime = 3f;
    public float rewindSpeed = 1f;
    public Transform lastCheckpoint;

    [Header("Effect References")]
    [SerializeField] private RewindEffectController rewindEffects;
    [SerializeField] private ParticleSystem rewindParticles;
    [SerializeField] private AudioSource rewindSound;

    // Components
    private Rigidbody rb;
    private Animator animator;
    private PlayerMovement playerMovement;
    private List<RewindFrame> movementHistory = new List<RewindFrame>();
    public bool isRewinding;

    // Event declarations
    public delegate void RewindAction();
    public event RewindAction OnRewindStart;
    public event RewindAction OnRewindComplete;

    private struct RewindFrame
    {
        public Vector3 position;
        public Quaternion rotation;
        public int animationStateHash;
        public bool wasMoving;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        if (rewindEffects == null)
            rewindEffects = FindObjectOfType<RewindEffectController>();
    }

    void FixedUpdate()
    {
        if (!isRewinding)
        {
            while (movementHistory.Count >= Mathf.CeilToInt(recordTime / Time.fixedDeltaTime))
                movementHistory.RemoveAt(0);

            movementHistory.Add(new RewindFrame()
            {
                position = transform.position,
                rotation = transform.rotation,
                animationStateHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash,
                wasMoving = playerMovement.moveDirection.magnitude > 0.1f
            });
        }
    }

    public void StartRewind()
    {
        if (!isRewinding)
        {
            if (movementHistory.Count > 0)
            {
                StartCoroutine(ExecuteRewind());
            }
            else if (lastCheckpoint != null)
            {
                transform.position = lastCheckpoint.position;
            }
        }
    }

    IEnumerator ExecuteRewind()
    {
        isRewinding = true;
        playerMovement.enabled = false;
        rb.isKinematic = true;

        OnRewindStart?.Invoke();

        bool wasMovingAtStart = movementHistory[movementHistory.Count - 1].wasMoving;
        animator.SetFloat("Speed", 1);
        animator.SetBool("isDashing", false);
        animator.SetFloat("Speed", wasMovingAtStart ? 1f : 0f);

        if (rewindEffects != null) rewindEffects.StartRewindEffects();
        if (rewindParticles != null) rewindParticles.Play();
        if (rewindSound != null) rewindSound.Play();

        for (int i = movementHistory.Count - 1; i >= 0; i--)
        {
            float t = 0;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            RewindFrame targetFrame = movementHistory[i];

            while (t < 1f)
            {
                t += Time.deltaTime * rewindSpeed;

                transform.position = Vector3.Lerp(startPos, targetFrame.position, t);
                transform.rotation = Quaternion.Slerp(startRot, targetFrame.rotation, t);

                animator.SetFloat("Speed", 1);
                animator.Play(targetFrame.animationStateHash, 0, Mathf.Lerp(1, 0, t));

                yield return null;
            }
        }

        rb.isKinematic = false;
        playerMovement.enabled = true;
        isRewinding = false;
        movementHistory.Clear();

        
        animator.SetBool("isDashing", false);
        animator.SetFloat("Speed", 0f);

        if (rewindEffects != null) rewindEffects.StopRewindEffects();
        if (rewindParticles != null) rewindParticles.Stop();

        OnRewindComplete?.Invoke();
    }
}