using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CannonInteraction : MonoBehaviour
{
    [Header("Cannon Settings")]
    public Rigidbody ball;
    public float launchForce = 10f;
    public GameObject rocksGroup;
    public float delayBeforeDisappearing = 1f;
    public GameObject targetObject;
    public GameObject targetObject1;

    [Header("UI Settings")]
    public Text interactionPrompt;
    public string promptText = "Press E to launch";
    public string missingFragmentText = "Need fragment!";

    private GameObject player;
    private bool isPlayerInRange = false;
    public MovementSlowZone timeZone;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            // Only show prompt if player has fragment
            if (inventory != null && inventory.hasFragment)
            {
                // Show "Press E to launch" prompt when in range and has fragment
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LaunchCannon();
                    if (timeZone != null)
                    {
                        timeZone.ResetPlayerSpeed();
                        Destroy(timeZone.gameObject);
                    }
                }
            }
            else
            {
                // Show "Need fragment!" prompt when the player doesn't have the fragment
                UpdatePromptVisibility();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UpdatePromptVisibility();  // Update the prompt when the player enters the trigger
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false); // Hide prompt when player exits trigger
        }
    }

    void UpdatePromptVisibility()
    {
        if (interactionPrompt != null && player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            // Only show the prompt if the player is in range
            interactionPrompt.gameObject.SetActive(isPlayerInRange);

            // Update the prompt text based on whether the player has the fragment
            if (inventory != null && inventory.hasFragment)
            {
                interactionPrompt.text = promptText; // Show "Press E to launch" if the player has the fragment
            }
            else
            {
                interactionPrompt.text = missingFragmentText; // Show "Need fragment!" if the player doesn't have the fragment
            }
        }
    }

    void LaunchCannon()
    {
        LaunchBall();
        if (targetObject != null) targetObject.SetActive(false);
        if (targetObject1 != null) targetObject1.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false); // Hide prompt after launching cannon
    }

    void LaunchBall()
    {
        ball.isKinematic = false;
        ball.useGravity = true;
        ball.AddForce(-transform.forward * launchForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BigRock"))
        {
            ball.gameObject.SetActive(false);
            ActivateRockGroup();
            StartCoroutine(DeactivateRocksAfterDelay());
        }
    }

    void ActivateRockGroup()
    {
        if (rocksGroup != null)
        {
            rocksGroup.SetActive(true);
            Rigidbody[] rockRigidbodies = rocksGroup.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rockRigidbodies)
            {
                rb.useGravity = true;
            }
        }
    }

    IEnumerator DeactivateRocksAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDisappearing);
        if (rocksGroup != null)
        {
            rocksGroup.SetActive(false);
        }
    }
}
