using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamurailHealth : MonoBehaviour
{
    
public int maxHealth = 2;  // Start with 2 health
    private int currentHealth;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Set animator parameter at start
        if (animator != null)
        {
            animator.SetInteger("Health", currentHealth);
        }
    }

    public void TakeHit()
    {
        currentHealth--;

        Debug.Log("Samurai hit! Current health: " + currentHealth);

        if (animator != null)
        {
            animator.SetInteger("Health", currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Samurai has died!");

        if (animator != null)
        {
            animator.SetTrigger("Die"); // Assuming you have a death animation
        }

        enemyFollow followScript = GetComponent<enemyFollow>();
    if (followScript != null)
    {
        followScript.enabled = false;
    }

    SamuraiShoot shootScript = GetComponent<SamuraiShoot>();
    if (shootScript != null)
    {
        shootScript.enabled = false;
    }

        // Destroy after animation (optional)
        Destroy(gameObject, 2f);
    }
}
