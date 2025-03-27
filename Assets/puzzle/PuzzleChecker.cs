using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public LetterSlot[] slots; // Slots where letters are placed
    public string[] correctAnswer = { "A", "B", "C" }; // Correct order
     public GameObject sword;
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
                return; // Stop checking if slots are empty
            }

            placedLetters[i] = slots[i].currentLetter.letter.Trim().ToUpper();
        }

        // Convert arrays to single string for easier debugging
        string placedString = string.Join("", placedLetters);
        string correctString = string.Join("", correctAnswer).ToUpper();

        Debug.Log("Placed: " + placedString + " | Expected: " + correctString);

        // Compare placed letters with correct answer
        if (placedString == correctString)
        {    sword.SetActive(true); // Enables the sword when the puzzle is solved

            Debug.Log("✅ Puzzle Solved! Unlock the next part.");
        }
        else
        {
            Debug.Log("❌ Incorrect! Try Again.");
        }
    }
}
