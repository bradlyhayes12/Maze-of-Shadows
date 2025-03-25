using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f; // Adjustable move speed

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake() {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Get input in Update (for responsiveness)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize so diagonal movement isn't faster
        movement = movement.normalized;
    }

    void FixedUpdate() {
        // Use MovePosition to move the rigidbody and let physics handle collisions
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
