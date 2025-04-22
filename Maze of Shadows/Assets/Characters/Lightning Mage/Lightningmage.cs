using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightningmage : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterMovement movementScript;

    private bool isAttacking = false;
    private bool isCasting = false;

    private int hitCount = 0;
    private bool isDead = false;

    [Header("Health Settings")]
    public int Health = 5;

    [Header("Melee Attack")]
    public float attackDuration = 1f; // Length of the melee animation
    public float attackCoolDown = 2f;

    [Header("Lightning Attack")]
    public float lightningCastDuration = 1f;
    public float lightningCoolDown = 4f;

    [Header("Projectile Settings")]
    public GameObject LightningBoltPrefab;
    public Transform lightningPoint;
    public float lightningSpeed = 5f;

    public MeleeHitbox swordHitbox;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movementScript = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        if (isDead) return;

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


        if (swordHitbox != null)
            swordHitbox.EnableHitbox();

        animator.SetTrigger("Melee");

        yield return new WaitForSeconds(attackDuration);

        if (swordHitbox != null)
            swordHitbox.DisableHitbox();

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

    public void ShootLightningBolt()
    {
        if (LightningBoltPrefab != null && lightningPoint != null)
        {
            Vector3 spawnPos = lightningPoint.position;
            Quaternion spawnRot = lightningPoint.rotation;
            // Spawn Lightning Bolt
            GameObject lightningbolt = Instantiate(LightningBoltPrefab, spawnPos, spawnRot);

            // Grab Components
            var projSR = lightningbolt.GetComponent<SpriteRenderer>();
            var projRB = lightningbolt.GetComponent<Rigidbody2D>();

            // Decide direction from our sprite renderer
            bool facingLeft = spriteRenderer.flipX;
            float dir = facingLeft ? -1 : +1;
            
            // Set Velocity
            if (projRB != null)
            {
                projRB.velocity = new Vector2(dir * lightningSpeed, 0f);
            }

            if (projSR != null)
            {
                projSR.flipX = facingLeft;
                projSR.sortingLayerName = "Projectiles";
                projSR.sortingOrder = 500;
            }
        }

           
    }

    public void TakeHit()
    {
        if (isDead) return;

        hitCount++;
        Debug.Log("Lightning Mage hit! Current hits: " + hitCount);

        if (hitCount >= Health)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
    }
}
