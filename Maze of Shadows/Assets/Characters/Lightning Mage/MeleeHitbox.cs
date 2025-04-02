using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private bool canHit = false;

    public void EnableHitbox()
    {
        canHit = true;
    }

    public void DisableHitbox()
    {
        canHit = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Hit enemy: " + other.name);
            // TODO: Damage enemy script here
        }
    }
}
