using UnityEngine;

public class FragmentPickup : MonoBehaviour
{
    public GameObject targetObject; // Optional object to disable
    private bool isPlayerInTrigger = false;

    void Update()
    {
        // Check for E key press ONLY when player is in trigger
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TryPickupFragment();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    void TryPickupFragment()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            if (targetObject != null) 
            {
                targetObject.SetActive(false);
            }

            inventory.hasFragment = true;
            Debug.Log("🔷 Fragment picked up!");
            Destroy(gameObject); // Remove the fragment
        }
    }
}