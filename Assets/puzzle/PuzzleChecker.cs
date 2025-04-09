using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public LetterSlot[] slots; // Slots where letters are placed
    public string[] correctAnswer = { "A", "B", "C" }; // Correct order
    public GameObject sword;
    public GameObject stone;
    bool isActivated;
    
    // Stone rotation settings
    public float rotationSpeed = 2f;
    public Vector3 targetRotation = new Vector3(0, 180, 0);
    private Quaternion originalStoneRotation;
    private Quaternion stoneTargetRotation;

    private void Start()
    {
        // Store original stone rotation
        if (stone != null)
        {
            originalStoneRotation = stone.transform.rotation;
            stoneTargetRotation = originalStoneRotation;
        }
    }

    private void Update()
    {
        if (stone != null)
        {
            // Rotate the stone smoothly
            stone.transform.rotation = Quaternion.Lerp(
                stone.transform.rotation,
                stoneTargetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    public void CheckSolution()
    {
        // Store placed letters
        string[] placedLetters = new string[slots.Length];

        // Check if all slots are filled
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentLetter == null)
            {
                Debug.Log("Some slots are empty, waiting for more letters...");
                return;
            }

            placedLetters[i] = slots[i].currentLetter.letter.Trim().ToUpper();
        }

        // Convert arrays to single string for comparison
        string placedString = string.Join("", placedLetters);
        string correctString = string.Join("", correctAnswer).ToUpper();

        Debug.Log("Placed: " + placedString + " | Expected: " + correctString);

        // Compare placed letters with correct answer
        if (placedString == correctString)
        {    
            if (sword != null)
                sword.SetActive(true);
            
            isActivated = true;
            
            if (stone != null)
                stoneTargetRotation = Quaternion.Euler(targetRotation);
            
            Debug.Log("✅ Puzzle Solved! Unlock the next part.");
        }
        else
        {
            Debug.Log("❌ Incorrect! Try Again.");
        }
    }
}