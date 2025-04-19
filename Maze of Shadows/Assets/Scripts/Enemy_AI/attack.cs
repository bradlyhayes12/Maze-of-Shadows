using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
    public float attackRange = 5f;         // Distance within which the enemy should attack
    public float attackCooldown = 2f;        // Time between attacks
    public float attackDuration = 1f;        // Duration to remain in attack state
    public int damage = 1;                   // Amount of damage to deal

    private Animator animator;
    private Transform player;
    private float nextAttackTime = 0f;
    private bool attackActive = false;       // Flag for whether we're currently attacking
    private bool damageAppliedThisAttack = false; // To ensure damage is applied only once per attack cycle

    void Start() {
        animator = GetComponent<Animator>();

        // Find the player using the "Player" tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) {
            player = playerObj.transform;
        } else {
            Debug.LogError("Player not found! Make sure your player is tagged as 'Player'.");
        }
    }

    void Update() {
        if (player != null) {
            float distance = Vector2.Distance(transform.position, player.position);

            // Trigger an attack if the player is in range, the cooldown has passed, and we're not already attacking
            if (distance <= attackRange && Time.time >= nextAttackTime && !attackActive) {
                nextAttackTime = Time.time + attackCooldown;
                StartCoroutine(AttackCoroutine());
                Debug.Log("Enemy is attacking!");
            }
        }
    }

    IEnumerator AttackCoroutine() {
        attackActive = true;
        damageAppliedThisAttack = false; // Reset for this attack cycle
        animator.SetBool("isAttacking", true);
        // Remain in attack state for the attack duration
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("isAttacking", false);
        attackActive = false;
    }

    // Use OnTriggerStay2D to continuously check if the player is in the attack area.
    void OnTriggerStay2D(Collider2D other) {
        Debug.Log("OnTriggerStay2D called with: " + other.name + ", attackActive: " + attackActive);
        if (other.CompareTag("Player") && attackActive && !damageAppliedThisAttack) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit! Damage: " + damage);
                damageAppliedThisAttack = true; // Prevent multiple hits in one attack cycle
            }
        }
         //only during the active attack window, only once per swing
    }
}
