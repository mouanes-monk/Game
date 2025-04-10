using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float wallStopThreshold = 0.1f;
    
    [Header("References")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider playerCollider;
    
    private bool isDashing;
    private bool isOnCooldown;
    private Vector3 dashDirection;
    private float currentCooldown;

    void Update()
    {
        if (GetComponent<PlayerRewind>()?.isRewinding == true) return;
        
        // Handle cooldown
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                isOnCooldown = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && !isOnCooldown)
        {
            dashDirection = (player.moveDirection.magnitude > 0.1f) 
                ? player.moveDirection.normalized 
                : transform.forward;
            
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("isDashing", true);
        
        float remainingDistance = dashDistance;
        Vector3 startPosition = transform.position;
<<<<<<< Updated upstream
=======
        
        // Determine dash direction - use movement if available, otherwise use facing direction
        Vector3 dashDirection = (player.MoveDirection.magnitude > 0.1f) 
            ? player.MoveDirection.normalized 
            : transform.forward;
        
        Vector3 endPosition = startPosition + dashDirection * dashDistance;
>>>>>>> Stashed changes

        while (remainingDistance > 0 && isDashing)
        {
            float moveDistance = Mathf.Min(dashSpeed * Time.deltaTime, remainingDistance);
            Vector3 proposedPosition = transform.position + dashDirection * moveDistance;
            
            if (Physics.CheckBox(
                proposedPosition + playerCollider.center, 
                playerCollider.size * 0.5f * (1 - wallStopThreshold),
                transform.rotation, 
                collisionLayers))
            {
                if (Physics.BoxCast(
                    transform.position + playerCollider.center, 
                    playerCollider.size * 0.5f, 
                    dashDirection, 
                    out RaycastHit hit, 
                    transform.rotation, 
                    moveDistance * 2f,
                    collisionLayers))
                {
                    transform.position = hit.point - dashDirection * 
                        (playerCollider.size.magnitude * 0.5f + wallStopThreshold);
                }
                break;
            }

            transform.position = proposedPosition;
            remainingDistance -= moveDistance;
            yield return null;
        }

        isDashing = false;
        animator.SetBool("isDashing", false);
        
        // Start cooldown
        isOnCooldown = true;
        currentCooldown = dashCooldown;
    }

    public bool CanDash()
    {
        return !isDashing && !isOnCooldown;
    }

    void OnDrawGizmos()
    {
        if (playerCollider != null && isDashing)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + playerCollider.center, 
                transform.rotation, 
                Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, playerCollider.size * (1 - wallStopThreshold));
        }
    }
}