using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public float speed = 3f;
    public float desiredSeparation = 3f;    // Minimum distance between enemies
    public float separationStrength = 5f;   // How strongly enemies repel each other

    private Animator animator;
    private Transform player;
    private Vector2 previousPosition;
    private SpriteRenderer spriteRenderer;

    void Start() {
        // Get the SpriteRenderer attached to this enemy
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Find the player using its tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) {
            player = playerObj.transform;
        } else {
            Debug.LogError("Player not found! Ensure your player is tagged as 'Player'.");
        }
        previousPosition = transform.position;
    }

    void Update() {
        if (player != null) {
            // Calculate base direction towards the player
            Vector3 baseDirection = (player.position - transform.position).normalized;
            
            // Calculate separation force to avoid clustering with other enemies
            Vector3 separationForce = Vector3.zero;
            EnemyMovement[] allEnemies = FindObjectsOfType<EnemyMovement>();
            foreach (EnemyMovement other in allEnemies) {
                if (other == this) continue;  // Skip self

                Vector3 diff = transform.position - other.transform.position;
                float distance = diff.magnitude;
                if (distance < desiredSeparation && distance > 0f) {
                    separationForce += diff.normalized / distance;
                }
            }
            separationForce *= separationStrength;

            // Combine the base direction with the separation force and normalize the result
            Vector3 finalDirection = (baseDirection + separationForce).normalized;
            
            // Move the enemy using the final direction
            transform.position += finalDirection * speed * Time.deltaTime;
            
            // Check if the enemy has moved since last frame to set "isMoving"
            float distanceMoved = Vector2.Distance((Vector2)transform.position, previousPosition);
            if (distanceMoved > 0.001f) {
                animator.SetBool("isMoving", true);
            } else {
                animator.SetBool("isMoving", false);
            }
             
            // Flip sprite based on player's x position relative to enemy
            if (player.position.x < transform.position.x) {
                spriteRenderer.flipX = true;  // Enemy faces left
            } else {
                spriteRenderer.flipX = false; // Enemy faces right
            }
    
            previousPosition = transform.position;
        }
    }
}
