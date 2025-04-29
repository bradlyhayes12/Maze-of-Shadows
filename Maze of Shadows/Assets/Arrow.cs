using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 3f;
    private Animator animator;
    private bool hasHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) Destroy(gameObject,lifetime);

        // this will find *any* component on the Player that implements IDamageable
        var dmgReceiver = other.GetComponent<IDamageable>();
        if (dmgReceiver != null)
        {
            dmgReceiver.TakeHit();
        }
        Destroy(gameObject);
    }

}