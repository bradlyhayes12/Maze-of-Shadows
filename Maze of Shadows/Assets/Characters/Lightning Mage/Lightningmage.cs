using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightningmage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterMovement movementScript;

    private bool isAttacking = false;
    private bool isCasting = false;

    [Header("Melee Attack")]
    public float attackDuration = 1f; // Length of the melee animation
    public float attackCoolDown = 2f;

    [Header("Lightning Attack")]
    public float lightningCastDuration = 1f;
    public float lightningCoolDown = 4f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movementScript = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isCasting)
        {
            StartCoroutine(CastLightning());
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

    private IEnumerator CastLightning()
    {
        isCasting = true;

        if(movementScript != null) movementScript.enabled = false;
        if (animator != null) animator.SetBool("isMoving", false);

        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(lightningCastDuration);

        if(movementScript != null) movementScript.enabled = true;

        yield return new WaitForSeconds(lightningCoolDown - lightningCastDuration);
        isCasting = false;

    }
}
