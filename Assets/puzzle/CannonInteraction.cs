using System.Collections;
using UnityEngine;

public class CannonInteraction : MonoBehaviour
{
    public Rigidbody ball;
    public float launchForce = 10f;
    public GameObject rocksGroup;
    public float delayBeforeDisappearing = 1f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.hasFragment)
            {
                LaunchBall();
            }
            else
            {
                Debug.Log("❌ You need the fragment to launch!");
            }
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
