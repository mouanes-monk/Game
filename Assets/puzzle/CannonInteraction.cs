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

    void UpdatePromptVisibility()
    {
        if (interactionPrompt != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            
            // Only show prompt if player has fragment
            interactionPrompt.gameObject.SetActive(inventory != null && inventory.hasFragment);
            interactionPrompt.text = inventory != null && inventory.hasFragment ? promptText : missingFragmentText;
        }
    }

    void LaunchCannon()
    {
        LaunchBall();
        if (targetObject != null) targetObject.SetActive(false);
        if (targetObject1 != null) targetObject1.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
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