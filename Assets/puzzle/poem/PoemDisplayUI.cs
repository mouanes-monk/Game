using UnityEngine;
using TMPro;

public class PoemDisplayUI : MonoBehaviour
{
    [Header("Fragment Displays")]
    public GameObject fragment1Display;
    public GameObject fragment2Display;
    public TextMeshProUGUI progressText;

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.F;
    public float fragmentShowTime = 3f;

    void Start()
    {
        HideAllFragments();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDisplay();
        }
    }

    public void SetFragment(int fragmentID)
    {
        HideAllFragments();
        
        switch(fragmentID)
        {
            case 1:
                if (fragment1Display != null)
                {
                    fragment1Display.SetActive(true);
                    Invoke("HideAllFragments", fragmentShowTime);
                }
                break;
            case 2:
                if (fragment2Display != null)
                {
                    fragment2Display.SetActive(true);
                    Invoke("HideAllFragments", fragmentShowTime);
                }
                break;
        }
    }

    public void UpdateProgress(int collected, int total)
    {
        if (progressText != null)
        {
            progressText.text = $"Fragments: {collected}/{total}";
        }
    }

    void ToggleDisplay()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null) return;

        bool showFragment1 = inventory.HasFragment(1) && !fragment1Display.activeSelf;
        bool showFragment2 = inventory.HasFragment(2) && !fragment2Display.activeSelf;

        if (showFragment1 || showFragment2)
        {
            fragment1Display.SetActive(showFragment1);
            fragment2Display.SetActive(showFragment2);
        }
        else
        {
            HideAllFragments();
        }
    }

    void HideAllFragments()
    {
        if (fragment1Display != null) fragment1Display.SetActive(false);
        if (fragment2Display != null) fragment2Display.SetActive(false);
    }
}