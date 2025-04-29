using UnityEngine;

public class ArrowDamage : MonoBehaviour
{
    public int damageAmount = 1; // How much health to remove (1 by default)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we hit has a WandererMagician script
        WandererMagican player = collision.GetComponent<WandererMagican>();
        if (player != null)
        {
            // Player got hit!
            player.TakeHit();
            Debug.Log("Arrow hit the player!");

            // Destroy the arrow after hitting
            Destroy(gameObject);
        }
        else
        {
            // Optionally: destroy arrow if it hits ANYTHING else too (like walls)
            // Destroy(gameObject);
        }
    }
}
