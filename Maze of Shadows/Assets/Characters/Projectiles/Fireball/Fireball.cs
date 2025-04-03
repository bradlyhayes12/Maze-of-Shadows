using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    // Add hit logic (like damage, explosion, etc.)
    //    Destroy(gameObject); // Destroy on hit
    //}

    public void DestroySelf(AnimationEvent evt = null)
    {
        Destroy(gameObject);
    }
}