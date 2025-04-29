using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime = 3f;
    private Animator animator;
    private bool hasHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        // this will find *any* component on the Player that implements IDamageable
        var dmgReceiver = other.GetComponent<SamurailHealth>();
        if (dmgReceiver != null)
        {
            dmgReceiver.TakeHit();
        }
        Destroy(gameObject);
    }

    public void DestroySelf(AnimationEvent evt = null)
    {
        Destroy(gameObject);
    }
}