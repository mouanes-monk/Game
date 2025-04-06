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
    public Text interactionPrompt; // Assign a UI Text element in Inspector
    public string promptText = "Press E to launch";
    public string missingFragmentText = "Need fragment!";

    private GameObject player;
    private bool isPlayerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && player != null)
        {
            TryLaunchCannon();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UpdatePromptVisibility();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
        }
    }

    void TryLaunchCannon()
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        
        if (inventory != null && inventory.hasFragment)
        {
            LaunchBall();
            if (targetObject != null) targetObject.SetActive(false);
             if (targetObject1 != null) targetObject1.SetActive(false);

            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("❌ You need the fragment to launch!");
            if (interactionPrompt != null)
            {
                interactionPrompt.text = missingFragmentText;
                StartCoroutine(ResetPromptText(2f));
            }
        }
    }

    IEnumerator ResetPromptText(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (interactionPrompt != null) interactionPrompt.text = promptText;
    }

    void UpdatePromptVisibility()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(true);
            interactionPrompt.text = promptText;
        }
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