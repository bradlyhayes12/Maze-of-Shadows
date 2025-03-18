using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
         // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Check if character is moving
        bool isMoving = movement.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        // Flip the character when moving left or right
        if (movement.x > 0)
        {
            spriteRenderer.flipX = true; // Face right
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = false; // Face left
        }

        //   If we want rotation we use this
        // if (movement != Vector2.zero)
        // {
        //     float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        //     transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust based on sprite orientation
        // }
    }

    void FixedUpdate()
    {
        rb.velocity = movement.normalized * moveSpeed; 
    }
}
