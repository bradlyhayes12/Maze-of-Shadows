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

        // Flip character by scaling, not by spriteRenderer.flipX
        if (movement.x > 0)
        {
            // Face right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Make sure x is positive
            transform.localScale = scale;
        }
        else if (movement.x < 0)
        {
            // Face left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // Make sure x is negative
            transform.localScale = scale;
        }

    }

        void FixedUpdate()
    {
          rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}