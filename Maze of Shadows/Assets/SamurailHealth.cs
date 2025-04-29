using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamurailHealth : MonoBehaviour
{
    public int maxHealth = 1;  // fallback if Animator doesn't have Health param
    private int currentHealth;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            int animatorHealth = animator.GetInteger("health");

            if (animatorHealth > 0)
            {
                maxHealth = animatorHealth;
            }
        }

        currentHealth = maxHealth;

        // Sync Animator parameter to our starting health
        if (animator != null)
        {
            animator.SetInteger("health", currentHealth);
        }
    }

    public void TakeHit()
    {
        if (currentHealth <= 0) return; // Already dead, ignore hits

        currentHealth--;

        Debug.Log("Samurai hit! Current health: " + currentHealth);

        if (animator != null)
        {
            animator.SetInteger("health", currentHealth); // Update Animator parameter to match new health
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
            animator.SetTrigger("dead"); // Play death animation trigger
        }

        // Disable AI behavior
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

        // Destroy Samurai after death animation delay
        Destroy(gameObject, 2f);
    }
}
