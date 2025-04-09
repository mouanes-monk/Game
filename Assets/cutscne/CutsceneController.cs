using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Scene References")]
    public GameObject player;
    public Animator playerAnimator;
    public GameObject timeMage;
    public GameObject portal;
    public Transform meetingPoint;
    public Transform portalPoint;

    [Header("Timing Settings")]
    public float initialDelay = 1f;
    public float playerMoveSpeed = 3f;
    public float typingSpeed = 0.05f;
    public float fastTypingSpeed = 0.01f;
    public KeyCode advanceKey = KeyCode.Space;

    private bool isTyping = false;
    private bool skipTyping = false;
    private bool isPlayerMoving = false;
    private Coroutine moveCoroutine;

    void Start()
    {
        player.GetComponent<MonoBehaviour>().enabled = false;
        timeMage.SetActive(false);
        portal.SetActive(false);
        dialoguePanel.SetActive(true);
        StartCoroutine(PlayIntroCutscene());
    }

    void Update()
    {
        if (Input.GetKeyDown(advanceKey))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
        }
    }

    IEnumerator PlayIntroCutscene()
    {
        yield return new WaitForSeconds(initialDelay);
        
        // Opening sequence
        yield return ShowDialogue("Narrator", "A presence awakens before you...");
        timeMage.SetActive(true);
        
        // Time Mage dialogue
        yield return ShowDialogue("???", "Ah... you stir at last, Antarah ibn Shaddad.");
        yield return MovePlayerTo(meetingPoint.position);
        yield return ShowDialogue("???", "Your name echoes through verses and battlefields long buried by time.");
        yield return ShowDialogue("???", "But now, even time forgets itself.");

        // Antara responses
        yield return ShowDialogue("Antara", "...Who are you?");
        yield return ShowDialogue("Antara", "What is this place? A dream?");

        // Time Mage exposition
        yield return ShowDialogue("???", "Not a dream... but not quite reality either.");
        yield return ShowDialogue("???", "This realm lies between seconds—held together by my will alone.");
        yield return ShowDialogue("???", "But I am fading. The flow of time is a storm... and I cling to its last thread.");
        yield return ShowDialogue("???", "The sands of the ages spiral in chaos. Ancient empires collapse into unborn futures.");
        yield return ShowDialogue("???", "I once held time's river in balance. Until the Destroyer came.");
        yield return ShowDialogue("???", "Our battle shattered the cycle. Now... past and future bleed into one another.");

        // Mission briefing
        yield return ShowDialogue("Antara", "You summoned me... to fix time?");
        yield return ShowDialogue("Time Mage", "Yes. I reached across the shattered veil to find a soul of strength and purpose.");
        yield return ShowDialogue("Time Mage", "You, Antara—born of fire and poetry. Warrior of blood and verse.");
        yield return ShowDialogue("Time Mage", "Scattered across the broken world lie the Time Fragments—Stop, Rewind, and Skip.");
        yield return ShowDialogue("Time Mage", "Only by gathering them can the spiral be mended.");

        // Portal sequence
        portal.SetActive(true);
        yield return ShowDialogue("Time Mage", "The portal awaits. Through it lies your first trial.");
        yield return ShowDialogue("Antara", "...So be it. If fate summoned me—then fate shall witness my blade.");
        yield return ShowDialogue("Time Mage", "May your sword remember its stories... and your heart defy the void.");

        // Final movement and scene transition
        yield return MovePlayerTo(portalPoint.position);
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator MovePlayerTo(Vector3 targetPosition)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        
        moveCoroutine = StartCoroutine(MoveToPosition(targetPosition));
        yield return moveCoroutine;
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isPlayerMoving = true;
        playerAnimator.SetBool("IsWalking", true);

        // Face direction first
        Vector3 direction = (targetPosition - player.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationTime = 0.5f;
            float elapsedRotationTime = 0f;
            
            while (elapsedRotationTime < rotationTime)
            {
                player.transform.rotation = Quaternion.Slerp(
                    player.transform.rotation,
                    targetRotation,
                    elapsedRotationTime / rotationTime
                );
                elapsedRotationTime += Time.deltaTime;
                yield return null;
            }
        }

        // Move to position
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                targetPosition,
                playerMoveSpeed * Time.deltaTime
            );
            yield return null;
        }

        playerAnimator.SetBool("IsWalking", false);
        isPlayerMoving = false;
    }

    IEnumerator ShowDialogue(string speaker, string text)
    {
        isTyping = true;
        skipTyping = false;
        
        string fullText = $"<b>{speaker}</b>: {text}";
        dialogueText.text = $"<b>{speaker}</b>: ";
        
        // Improved typing effect
        float currentTypingSpeed = typingSpeed;
        int visibleCharacters = 0;
        bool richTextTag = false;

        while (visibleCharacters < fullText.Length)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }

            // Handle rich text tags
            if (fullText[visibleCharacters] == '<')
            {
                richTextTag = true;
            }

            visibleCharacters++;
            dialogueText.text = fullText.Substring(0, visibleCharacters);

            if (richTextTag)
            {
                if (fullText[visibleCharacters - 1] == '>')
                {
                    richTextTag = false;
                }
                continue;
            }

            // Speed up if holding advance key
            currentTypingSpeed = Input.GetKey(advanceKey) ? fastTypingSpeed : typingSpeed;
            yield return new WaitForSeconds(currentTypingSpeed);
        }

        dialogueText.text = fullText + "\n\n<color=#FFD700>► Press Space</color>";
        isTyping = false;

        // Wait for advance input
        yield return new WaitUntil(() => Input.GetKeyDown(advanceKey));
        yield return new WaitForSeconds(0.1f); // Input buffer
    }
}