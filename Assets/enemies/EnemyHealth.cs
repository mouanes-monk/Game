using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        Debug.Log(this.health);
        if (this.health < 0)
        {
            Debug.Log("Enemy dead");
        }
    }
}
