using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = .0005f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = Mathf.Abs(movement.x) > 0 || Mathf.Abs(movement.y) > 0;
        animator.SetBool("isMoving", isMoving);

        // Flip the character left/right instantly
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false; // Face right (usually false is right)
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
    }

        void FixedUpdate()
    {
          rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}