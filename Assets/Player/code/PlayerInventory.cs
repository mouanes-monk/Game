using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Time Fragment")]
    public bool hasTimeFragment = false;

    [Header("Poem Fragments")]
    public PoemFragmentData[] poemFragments = new PoemFragmentData[2];

    [Header("UI Reference")]
    public PoemDisplayUI poemUI;

    [System.Serializable]
    public class PoemFragmentData
    {
        public bool collected = false;
    }

    // Simple property to check if any fragment is collected
    public bool hasFragment
    {
        get { return hasTimeFragment; }
        set { hasTimeFragment = value; }
    }

    public void AddTimeFragment()
    {
        hasTimeFragment = true;
    }

    public void AddPoemFragment(int fragmentID)
    {
        if (fragmentID < 1 || fragmentID > poemFragments.Length) return;

        int index = fragmentID - 1;

        if (!poemFragments[index].collected)
        {
            poemFragments[index].collected = true;

            if (poemUI != null)
            {
                poemUI.SetFragment(fragmentID);
                poemUI.UpdateProgress(GetCollectedCount(), poemFragments.Length);
            }
        }
    }

    public bool HasFragment(int fragmentID = 0)
    {
        if (fragmentID == 0) return hasTimeFragment;
        if (fragmentID > 0 && fragmentID <= poemFragments.Length)
            return poemFragments[fragmentID - 1].collected;
        return false;
    }

    private int GetCollectedCount()
    {
        int count = 0;
        foreach (var fragment in poemFragments)
        {
            if (fragment.collected) count++;
        }
        return count;
    }
}