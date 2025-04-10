using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TimeStop : MonoBehaviour
{
    public float freezeDuration = 4f;

    public void FreezeEnemies(float duration)
    {
        StartCoroutine(FreezeAllEnemies(duration));
    }

    private IEnumerator FreezeAllEnemies(float duration)
    {
        enemyBehavior[] enemies = FindObjectsOfType<enemyBehavior>();

        foreach (enemyBehavior enemy in enemies)
        {
            enemy.enabled = false; // Disable enemy script
            enemy.GetComponent<NavMeshAgent>().enabled = false;
        }

        yield return new WaitForSeconds(duration);

        foreach (enemyBehavior enemy in enemies)
        {
            enemy.enabled = true; // Re-enable enemy script
            enemy.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

