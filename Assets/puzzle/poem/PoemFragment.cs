using UnityEngine;

public class PoemFragment : MonoBehaviour
{
    public int fragmentID = 1;
    public GameObject pickupEffect;
    public KeyCode collectKey = KeyCode.E;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(collectKey))
        {
            TryCollectFragment();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void TryCollectFragment()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null && !inventory.HasFragment(fragmentID))
        {
            Debug.Log($"Collected fragment {fragmentID}!");

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            inventory.AddPoemFragment(fragmentID);
            Destroy(gameObject);
        }
    }
}