using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public LetterBox currentLetter;
    public PuzzleChecker puzzleChecker;

 private void OnTriggerEnter(Collider other)
{
    LetterBox letterBox = other.GetComponent<LetterBox>();
    if (letterBox != null && currentLetter == null)
    {
        Debug.Log("[LetterSlot] Detected LetterBox: " + letterBox.letter);
        currentLetter = letterBox;

        if (puzzleChecker != null)
        {
            Debug.Log("[LetterSlot] Calling CheckSolution()...");
            puzzleChecker.CheckSolution();
        }
        else
        {
            Debug.LogError("[LetterSlot] puzzleChecker reference is NULL!");
        }
    }
}

    private void OnTriggerExit(Collider other)
    {
        LetterBox letterBox = other.GetComponent<LetterBox>();
        if (letterBox != null && currentLetter == letterBox)
        {
            // 🔹 Delay removal to ensure it really leaves the slot area
            Invoke(nameof(RemoveLetter), 0.1f); 
        }
    }

    private void RemoveLetter()
    {
        if (currentLetter != null) // Final check before removing
        {
            Debug.Log("Letter Removed: " + currentLetter.letter);
            currentLetter = null;

            if (puzzleChecker != null)
            {
                puzzleChecker.CheckSolution();
            }
        }
    }
}
