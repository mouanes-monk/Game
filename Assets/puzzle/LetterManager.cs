using UnityEngine;

public class LetterManager : MonoBehaviour
{
    public LetterBox[] letterBoxes; // Drag and drop all letter boxes here
    public string[] lettersToUse = { "A", "B", "C", "D", "E" }; // Customize your letters

    void Start()
    {
        if (letterBoxes.Length != lettersToUse.Length)
        {
            Debug.LogError("LetterBoxes and LettersToUse must have the same length!");
            return;
        }

        // Assign letters to each box
        for (int i = 0; i < letterBoxes.Length; i++)
        {
            letterBoxes[i].SetLetter(lettersToUse[i]); // Use the function from LetterBox
        }
    }
}
