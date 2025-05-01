using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    public float speed = 2.0f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2.0f;

    private Transform player;
    private Animator animator;
    private float lastAttackTime = -Mathf.Infinity;
    private float attackDuration = 0.5f; // How long isAttacking should stay true
    private float attackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure the player GameObject has the tag 'Player'.");
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x = (player.position.x > transform.position.x) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (distance > attackRange)
        {
            // Move toward player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            animator?.SetBool("isMoving", true);
            animator?.SetBool("isAttacking", false);
        }
        else
        {
            animator?.SetBool("isMoving", false);

            // Trigger attack if cooldown is over
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator?.SetBool("isAttacking", true);
                lastAttackTime = Time.time;
                attackTimer = attackDuration;
            }
        }

        // Turn off isAttacking after short delay
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                animator?.SetBool("isAttacking", false);
                animator?.SetBool("isMoving", true);
            }
        }
    }
}

    

