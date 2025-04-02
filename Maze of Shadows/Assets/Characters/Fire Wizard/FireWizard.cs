using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizard : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterMovement movementScript;  // Updated this line!

    public float fireballBurstTime = 1f;
    public float fireballCooldown = 5f;

    private bool canFire = true;

    public float attackDuration = 1f; // Length of the melee animation
    public float attackCoolDown = 2f;
    private bool isAttacking = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movementScript = GetComponent<CharacterMovement>(); // get movement script
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canFire)
        {
            StartCoroutine(FireballBurstRoutine());
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;

        //Stop movement while swinging
        if (movementScript != null)
            movementScript.enabled = false;

        if (animator != null)
            animator.SetBool("isMoving", false);

        animator.SetTrigger("Melee");

        yield return new WaitForSeconds(attackDuration);

        if (movementScript != null)
            movementScript.enabled = true;


        // Wait for animation to play (or use Animation Events if needed)
        yield return new WaitForSeconds(attackCoolDown - attackDuration);

        isAttacking = false;
    }

    private IEnumerator FireballBurstRoutine()
    {
        canFire = false;

        // Stop movement
        if (movementScript != null)
            movementScript.enabled = false;

        if (animator != null)
            animator.SetBool("isMoving", false);

        Debug.Log("Firing started");

        float timer = 0f;

        while (timer < fireballBurstTime)
        {
            animator.SetTrigger("Fireball");

            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        // Resume movement
        if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("Firing done, cooldown...");

        yield return new WaitForSeconds(fireballCooldown);

        canFire = true;

        Debug.Log("Fireball ready!");
    }
}