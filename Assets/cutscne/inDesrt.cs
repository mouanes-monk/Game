using System.Collections;
using UnityEngine;
using TMPro;

public class inDesrt : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Typing Settings")]
    public float initialDelay = 1f;
    public float typingSpeed = 0.05f;
    public float fastTypingSpeed = 0.01f;
    public KeyCode advanceKey = KeyCode.Space;

    private bool isTyping = false;
    private bool skipTyping = false;
    private bool hasEnteredCollider = false;

    void Start()
    {
        dialoguePanel.SetActive(true); // Show the dialogue panel when the game starts
        StartCoroutine(PlayIntroCutscene()); // Start the initial dialogues
    }

    void Update()
    {
        // Skip typing if the user presses the advance key
        if (Input.GetKeyDown(advanceKey) && isTyping)
        {
            skipTyping = true;
        }
    }

    // This method will handle the dialogue that starts when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCollider)
        {
            hasEnteredCollider = true; // Ensure the dialogue only triggers once
            StartCoroutine(ShowPuzzleDialogue()); // Show the puzzle-specific dialogue
        }
    }

    // This method handles the Time Mage's explanation when the player enters the trigger zone
    IEnumerator ShowPuzzleDialogue()
    {
        yield return new WaitForSeconds(0.5f); // Small delay before showing the puzzle dialogue

        // New dialogue explaining the puzzle task
        yield return ShowDialogue("Time Mage", "But be warned, Antara. There are riddles hidden in this world, puzzles woven into time itself.");
        yield return ShowDialogue("Time Mage", "Look for pieces of a forgotten poem scattered across this land. Each part will help you solve the riddle.");
        yield return ShowDialogue("Time Mage", "Solve the riddle to unblock the path ahead, and then proceed to the portal.");

        // Optionally add more dialogue after the puzzle explanation
        yield return ShowAutoDialogue("Antara", "A riddle, huh... I will solve it, for Abla’s sake.", 3f);
        yield return ShowAutoDialogue("Antara", "I can feel it... this task is important.", 3f);

        // Dialogue ends, wait for player action or any other trigger
        dialoguePanel.SetActive(false); // Hide the dialogue panel after the task explanation
    }

    // Coroutine for showing a single line of dialogue with typing effect
    IEnumerator ShowDialogue(string speaker, string text)
    {
        isTyping = true;
        skipTyping = false;

        string fullText = $"<b>{speaker}</b>: {text}";
        dialogueText.text = "";

        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }

            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    dialogueText.text += fullText.Substring(i, tagEnd - i + 1);
                    i = tagEnd + 1;
                    continue;
                }
            }

            dialogueText.text += fullText[i];
            i++;

            float currentSpeed = Input.GetKey(advanceKey) ? fastTypingSpeed : typingSpeed;
            yield return new WaitForSeconds(currentSpeed);
        }

        dialogueText.text = fullText + "\n\n<color=#FFD700>► Press Space</color>";
        isTyping = false;

        yield return new WaitUntil(() => Input.GetKeyDown(advanceKey));
        yield return new WaitForSeconds(0.1f);
    }

    // Coroutine for automatically displaying a line of dialogue with a delay
    IEnumerator ShowAutoDialogue(string speaker, string text, float stayDuration)
    {
        isTyping = true;

        string fullText = $"<b>{speaker}</b>: {text}";
        dialogueText.text = "";

        int i = 0;
        while (i < fullText.Length)
        {
            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    dialogueText.text += fullText.Substring(i, tagEnd - i + 1);
                    i = tagEnd + 1;
                    continue;
                }
            }

            dialogueText.text += fullText[i];
            i++;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(stayDuration);
        dialogueText.text = "";
        isTyping = false;
    }

    // This method plays the original dialogue at the start of the game
    IEnumerator PlayIntroCutscene()
    {
        yield return new WaitForSeconds(initialDelay);

        // Original dialogues you provided
        yield return ShowDialogue("Time Mage", "It seems time has bent itself, bringing us back to the golden age of the Arabic era.");
        yield return ShowDialogue("Antara", "This place... it feels like home, but something is off. The old man’s words... they were true.");
        yield return ShowDialogue("Time Mage", "I can sense a fragment of time suspended nearby. Explore this place and solve the puzzles woven by time itself. Finding this fragment will help you complete your mission.");
        yield return ShowDialogue("Time Mage", "Using the time fragment will stop the frozen cannon, and clear the path.");

        // Small delay before auto lines
        yield return new WaitForSeconds(2f);

        // Antara's emotional automatic lines
        yield return ShowAutoDialogue("Antara", "Abla... I wonder if you’re seeing this too.", 3f);
          yield return new WaitForSeconds(1f);
        yield return ShowAutoDialogue("Antara", "Your smile still echoes in my memory.", 2.5f);
          yield return new WaitForSeconds(30f);
        yield return ShowAutoDialogue("Antara", "I still remember her laughter across the dunes.", 3f);
          yield return new WaitForSeconds(40f);
        yield return ShowAutoDialogue("Antara", "I miss her more than time can carry.", 4f);

        dialoguePanel.SetActive(false);
    }
}
