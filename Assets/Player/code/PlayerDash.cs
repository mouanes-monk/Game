using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 1.0f;
    [SerializeField] private float dashDistance = 3.0f;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator animator;
    private bool Dashing;

    void Update()
    {if (GetComponent<PlayerRewind>().isRewinding) return;
        if (Input.GetKeyDown(KeyCode.Space) && !Dashing)
        {
            animator.SetBool("isDashing", true);
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        Dashing = true;
        Vector3 startPosition = transform.position;
        
        // Determine dash direction - use movement if available, otherwise use facing direction
        Vector3 dashDirection = (player.moveDirection.magnitude > 0.1f) 
            ? player.moveDirection.normalized 
            : transform.forward;
        
        Vector3 endPosition = startPosition + dashDirection * dashDistance;

        float dashTime = 0f;

        while (dashTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, dashTime);
            dashTime += Time.deltaTime * dashSpeed;
            yield return null;
        }

        transform.position = endPosition;
        Dashing = false;
        animator.SetBool("isDashing", false);
    }
}