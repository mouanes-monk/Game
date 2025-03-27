using UnityEngine;
using TMPro;

public class LetterBox : MonoBehaviour
{
    public string letter;
    private TMP_Text textMeshPro;
      public LetterSlot currentSlot = null;

    void Awake()
    {
        // Find TMP_Text component inside the child object
        textMeshPro = GetComponentInChildren<TMP_Text>();

        if (textMeshPro == null)
        {
            Debug.LogError("TMP_Text component not found in children of " + gameObject.name);
        }
    }

    public void SetLetter(string newLetter)
    {
        letter = newLetter;
        if (textMeshPro != null)
        {
            textMeshPro.text = newLetter;
        }
    }
}
