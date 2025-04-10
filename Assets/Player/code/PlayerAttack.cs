using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private SwordHitbox sword;


    [Header("Combat Settings")]
    [SerializeField] private int maxComboLength = 4;
    [SerializeField] private float comboCooldown = 1.5f;
    [SerializeField] private float comboWindowTime = 0.8f; // Time window to input next combo move

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField]
    private string[] comboStateNames = new string[4]
    {
        "Attack1",
        "Attack2",
        "KickAnimation",
        "FinalAnimation"
    };

    // Debug visualization
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private int currentComboCount = 0;
    [SerializeField] private bool isOnCooldown = false;

    // Private variables
    private int bufferedClicks = 0;
    private bool isPlayingCombo = false;
    private float cooldownTimer = 0f;
    private float comboWindowTimer = 0f;
    private bool comboWindowActive = false;

    // Events
    public delegate void ComboEvent(int comboCount);
    public event ComboEvent OnComboStarted;
    public event ComboEvent OnComboEnded;
    public event ComboEvent OnComboHit;

    private void Start()
    {
        
    }

    private void Update()
    {
        sword.DeactivateHitbox();
        // Handle cooldown state
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
            return; // Ignore all input during cooldown
        }

        // Handle combo window timing
        if (comboWindowActive)
        {
            comboWindowTimer -= Time.deltaTime;
            if (comboWindowTimer <= 0f)
            {
                // Window expired, execute the combo we have so far
                if (bufferedClicks > 0 && !isPlayingCombo)
                {
                    StartCoroutine(ExecuteCombo(bufferedClicks));
                }
                comboWindowActive = false;
            }
        }

        // Process player input
        if (Input.GetMouseButtonDown(0))
        {
            HandleAttackInput();
        }

        // Start combo if needed
        if (bufferedClicks > 0 && !isPlayingCombo && !comboWindowActive)
        {
            StartCoroutine(ExecuteCombo(bufferedClicks));
        }
    }

    private void HandleAttackInput()
    {
        // Don't accept input during cooldown
        if (isOnCooldown)
            return;

        // Buffer the click
        if (bufferedClicks < maxComboLength)
        {
            bufferedClicks++;

            // Start a combo window timer for chaining combos
            comboWindowTimer = comboWindowTime;
            comboWindowActive = true;

            // Visual feedback that click was registered
            if (showDebugInfo)
                Debug.Log($"Attack input registered. Current buffer: {bufferedClicks}");
        }
    }

    private IEnumerator ExecuteCombo(int comboLength)
    {
        animator.SetBool("isAttacking", true);
        isPlayingCombo = true;
        comboWindowActive = false;
        currentComboCount = comboLength;

        // Notify listeners that combo started
        OnComboStarted?.Invoke(comboLength);

        // Clear the buffer immediately to prevent additional queuing
        int hitsToPlay = bufferedClicks;
        bufferedClicks = 0;

        // Play each animation in sequence
        for (int i = 0; i < hitsToPlay && i < maxComboLength; i++)
        {
            // Get the state name for this combo position
            string stateName = comboStateNames[i];

            // Play the animation
            sword.ActivateHitbox();
            animator.Play(stateName, 0, 0f);

            // Notify listeners of this hit
            OnComboHit?.Invoke(i + 1);

            // Wait one frame for animation to start
            yield return null;

            // Then wait until this animation completes
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            while (state.IsName(stateName) && state.normalizedTime < 1f)
            {
                yield return null;
                state = animator.GetCurrentAnimatorStateInfo(0);
            }
        }

        // Combo completed, apply cooldown
        isPlayingCombo = false;
        isOnCooldown = true;
        cooldownTimer = comboCooldown;
        animator.SetBool("isAttacking", false);

        // Notify listeners combo has ended
        OnComboEnded?.Invoke(hitsToPlay);

        // Reset the combo counter after cooldown completes
        yield return new WaitForSeconds(comboCooldown);
        currentComboCount = 0;
    }


    public bool IsOnCooldown() => isOnCooldown;
    public bool IsInCombo() => isPlayingCombo;
    public float GetRemainingCooldown() => isOnCooldown ? cooldownTimer : 0f;
    public int GetCurrentComboLength() => currentComboCount;


    public void CancelCombo()
    {
        if (isPlayingCombo)
        {
            StopAllCoroutines();
            isPlayingCombo = false;
            bufferedClicks = 0;
            currentComboCount = 0;

            // Apply a shorter cooldown when interrupted
            isOnCooldown = true;
            cooldownTimer = comboCooldown * 0.5f;
        }
    }

    public void ActivateHitbox()
    {
        sword.ActivateHitbox();
    }

    public void DeactivateHitbox()
    {
        sword.DeactivateHitbox();
    }
}