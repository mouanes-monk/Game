using UnityEngine;

public class FragmentPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.hasFragment = true;
                Debug.Log("🔷 Fragment picked up!");
                Destroy(gameObject); // Remove the fragment
            }
        }
    }
}
