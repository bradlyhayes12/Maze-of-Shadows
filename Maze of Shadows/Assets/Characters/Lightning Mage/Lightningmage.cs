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

        if (movementScript != null) movementScript.enabled = false;
        if (animator != null) animator.SetBool("isMoving", false);

        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(lightningCastDuration);

        if (movementScript != null) movementScript.enabled = true;

        yield return new WaitForSeconds(lightningCoolDown - lightningCastDuration);
        isCasting = false;

    }

    public void ShootLightningBolt()
    {
        if (LightningBoltPrefab != null && lightningPoint != null)
        {
            GameObject lightningbolt = Instantiate(LightningBoltPrefab, lightningPoint.position, Quaternion.identity);

            var sr = lightningbolt.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Projectiles";
                sr.sortingOrder = 500;
            }

            Rigidbody2D rb = lightningbolt.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x > 0 ? 1 : -1;
                rb.velocity = new Vector2(lightningSpeed * direction, 0);

                if (direction < 0)
                {
                    Vector3 lightningScale = lightningbolt.transform.localScale;
                    lightningScale.x *= -1; // Flip horizontally
                    lightningbolt.transform.localScale = lightningScale;
                }
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
