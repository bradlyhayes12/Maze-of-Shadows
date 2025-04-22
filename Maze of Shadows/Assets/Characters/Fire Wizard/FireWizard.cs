using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizard : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CharacterMovement movementScript;

    private int hitCount = 0;
    private bool isDead = false;

    [Header("Health Settings")]
    public int Health = 5;

    [Header("Fireball Attack")]
    public float fireballBurstTime = 1f;
    public float fireballCooldown = 5f;
    private bool canFire = true;

    [Header("Projectile Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 5f;

    [Header("Melee Attack")]
    public float attackDuration = 1f;
    public float attackCoolDown = 2f;
    private bool isAttacking = false;

    public MeleeHitbox swordHitbox;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movementScript = GetComponent<CharacterMovement>();
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

        yield return new WaitForSeconds(attackCoolDown - attackDuration);
        isAttacking = false;
    }

    private IEnumerator FireballBurstRoutine()
    {
        canFire = false;

        if (movementScript != null)
            movementScript.enabled = false;

        if (animator != null)
            animator.SetBool("isMoving", false);

        Debug.Log("Firing started");

        float timer = 0f;

        while (timer < fireballBurstTime)
        {
            animator.SetTrigger("Fireball"); // The animation will call ShootFireball()
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("Firing done, cooldown...");

        yield return new WaitForSeconds(fireballCooldown);
        canFire = true;

        Debug.Log("Fireball ready!");
    }

    // Called from animation event
    public void ShootFireball()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

            var sr = fireball.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Projectiles";
                sr.sortingOrder = 500;
            }

            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x > 0 ? 1 : -1;
                rb.velocity = new Vector2(fireballSpeed * direction, 0);

                if (direction < 0)
                {
                    Vector3 fireScale = fireball.transform.localScale;
                    fireScale.x *= -1; // Flip horizontally
                    fireball.transform.localScale = fireScale;
                }
            }
        }
    }

    public void TakeHit()
    {
        if (isDead) return;

        hitCount++;
        Debug.Log("Fire Wizartd hit! Current hits: " + hitCount);

        if (hitCount >= Health)
        {
            Die();
        }
        else
        {
            Debug.Log("TakeHit() called, triggering Hurt");
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
