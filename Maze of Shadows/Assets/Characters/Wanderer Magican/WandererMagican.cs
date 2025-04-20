using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererMagican : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterMovement movementScript;

    private bool isAttacking = false;
    private bool isCasting = false;

    [Header("Health")]
    public int Health = 5;

    private int hitCount = 0;
    private bool isDead = false;

    [Header("Melee Attack")]
    public float attackDuration = 1f; // Length of the melee animation
    public float attackCoolDown = 2f;

    [Header("Projectile Settings")]
    public GameObject magicPrefab;
    public Transform magicPoint;
    public float magicSpeed = 5f;

    [Header("Magican Attack")]
    public float magicCastDuration = 2f;
    public float magicCoolDown = 4f;

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
            StartCoroutine(CastMagic());
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

    private IEnumerator CastMagic()
    {
        isCasting = true;

        if (movementScript != null) movementScript.enabled = false;
        if (animator != null) animator.SetBool("isMoving", false);

        animator.SetTrigger("Cast");

        yield return new WaitForSeconds(magicCastDuration);

        if (movementScript != null) movementScript.enabled = true;

        yield return new WaitForSeconds(magicCoolDown - magicCastDuration);
        isCasting = false;

    }

    public void ShootMagic()
    {
        if (magicPrefab != null && magicPoint != null)
        {
            // 1) Spawn the projectile
            GameObject magic = Instantiate(magicPrefab, magicPoint.position, Quaternion.identity);

            // 2) Immediately force it onto a higher Sorting Layer / Order
            var sr = magic.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Projectiles";  // make sure you’ve created this Sorting Layer
                sr.sortingOrder = 500;            // anything above your tilemap’s order
            }

            // 3) Give it its velocity (and flip if needed)
            Rigidbody2D rb = magic.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x > 0 ? 1 : -1;
                rb.velocity = new Vector2(magicSpeed * direction, 0);

                if (direction < 0)
                {
                    Vector3 magicScale = magic.transform.localScale;
                    magicScale.x *= -1; // Flip horizontally
                    magic.transform.localScale = magicScale;
                }
            }
        }
    }

    public void TakeHit()
    {
        if (isDead) return;

        hitCount++;
        Debug.Log("The magician hit! Current hits: " + hitCount);

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
