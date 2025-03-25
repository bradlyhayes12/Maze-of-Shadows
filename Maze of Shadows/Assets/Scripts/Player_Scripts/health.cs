using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 3;    // Maximum health (you can adjust this value)
    private int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    // Call this method to reduce the player's health
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health left: " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    // Handle player death (for now, simply reload the scene)
    void Die() {
        Debug.Log("Player died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
