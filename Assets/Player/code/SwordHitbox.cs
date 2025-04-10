using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordHitbox : MonoBehaviour
{
    private int hitCount = 0;
    public Collider swordCollider;

    void Awake()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false; 
    }
    private void OnTriggerEnter(Collider other)
    {
        hitCount++;
        if (other.CompareTag("Enemy") && hitCount != 3)
        {
            Debug.Log("Hit: " + other.name);
            other.GetComponent<EnemyHealth>()?.TakeDamage(30);
        }
        else
        {
            Debug.Log("Hit: " + other.name);
            other.GetComponent<EnemyHealth>()?.TakeDamage(40);
        }

        if (hitCount == 3)
        {
            hitCount = 0;
        }
    }

    public void ActivateHitbox()
    {
        swordCollider.enabled = true;
    }

    public void DeactivateHitbox()
    {
        swordCollider.enabled = false;
    }
}
