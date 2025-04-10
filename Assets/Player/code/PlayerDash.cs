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

    while (remainingDistance > 0f)
    {
        float moveDistance = Mathf.Min(dashSpeed * Time.deltaTime, remainingDistance);

        // Perform a box cast to check if there is a collider in the dash direction
        if (Physics.BoxCast(
            transform.position + playerCollider.center,
            playerCollider.size * 0.5f,
            dashDirection,
            out RaycastHit hit,
            transform.rotation,
            moveDistance,
            collisionLayers))
        {
            // Stop right before the collider
            float safeDistance = hit.distance - wallStopThreshold;
            if (safeDistance > 0f)
            {
                transform.position += dashDirection * safeDistance;
            }
            break;
        }

        // Move character if no obstacle detected
        transform.position += dashDirection * moveDistance;
        remainingDistance -= moveDistance;
        yield return null;
    }

    isDashing = false;
    animator.SetBool("isDashing", false);

    // Cooldown starts
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