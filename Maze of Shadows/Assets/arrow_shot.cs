using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_shot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CharacterMovement movementScript;

    private int hitCount = 0;
    private bool isDead = false;
    private Animator animator;
    [Header("Health Settings")]
    public int Health = 5;

    [Header("Arrow Attack")]
    public float arrowBurstTime = 1f;
    public float arrowCooldown = 5f;
    private bool canFire = true;

    [Header("Projectile Settings")]
    public GameObject arrowPrefab;
    public Transform arrowPoint;
    public float arrowSpeed = 5f;


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
            StartCoroutine(arrowShotRoutine());
        }

        
    }


    private IEnumerator arrowShotRoutine()
    {
        canFire = false;

        if (movementScript != null)
            movementScript.enabled = false;

        if (animator != null)
            animator.SetBool("isMoving", false);

        Debug.Log("Firing started");

        float timer = 0f;

        while (timer < arrowBurstTime)
        {
            animator.SetTrigger("arrow"); // The animation will call Shootarrow()
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("Firing done, cooldown...");

        yield return new WaitForSeconds(arrowCooldown);
        canFire = true;

        Debug.Log("arrow ready!");
    }

    // Called from animation event
    public void Shootarrow()
    {
        if (arrowPrefab != null && arrowPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowPoint.position, Quaternion.identity);

            var sr = arrow.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Projectiles";
                sr.sortingOrder = 500;
            }

            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x > 0 ? 1 : -1;
                rb.velocity = new Vector2(arrowSpeed * direction, 0);

                if (direction < 0)
                {
                    Vector3 fireScale = arrow.transform.localScale;
                    fireScale.x *= -1; // Flip horizontally
                    arrow.transform.localScale = fireScale;
                }
            }
        }
    }

    // public void TakeHit()
    // {
    //     if (isDead) return;

    //     hitCount++;
    //     Debug.Log("Fire Wizartd hit! Current hits: " + hitCount);

    //     if (hitCount >= Health)
    //     {
    //         Die();
    //     }
    //     else
    //     {
    //         Debug.Log("TakeHit() called, triggering Hurt");
    //         animator.SetTrigger("Hurt");
    //     }
    // }

    // private void Die()
    // {
    //     isDead = true;
    //     animator.SetTrigger("Death");
    //     if (movementScript != null)
    //     {
    //         movementScript.enabled = false;
    //     }
    // }
}
