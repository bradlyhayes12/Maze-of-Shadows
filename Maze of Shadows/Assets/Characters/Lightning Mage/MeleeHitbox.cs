using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{

    private Collider2D hitbox;

    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false; // Disable it at the start
    }

    public void EnableHitbox()
    {
        Debug.Log("Hitbox ENABLED");
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        Debug.Log("Hitbox DISABLED");
        hitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Hit wall: " + other.name);
        }
        
           

    }
}
