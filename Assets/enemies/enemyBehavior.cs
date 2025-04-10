using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBehavior : MonoBehaviour
{

    [SerializeField] private float distance;
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        distance = Vector3.Distance(this.transform.position, player.position);

        if (distance < 30)
        {
            agent.destination = player.position;
        }
    }
}
