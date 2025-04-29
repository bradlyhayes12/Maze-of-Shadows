using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    public float speed = 2.0f;
    public float attackRange = 1.5f;

    private Transform player;
    private Animator animator;

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

        // Flip sprite to face player
        Vector3 scale = transform.localScale;
        if (player.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x); // Face right
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // Face left
        }
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
            // Stop and attack
            animator?.SetBool("isMoving", false);
            animator?.SetBool("isAttacking", true);
        }
    }
}
    

