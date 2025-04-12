using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
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
        if (!hasHit)
        {
            hasHit = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        //// Add hit logic (like damage, explosion, etc.)
        //Debug.Log("Fireball hit: " + other.name); 
        //Destroy(gameObject); // Destroy on hit
        WandererMagican magican = other.GetComponent<WandererMagican>();
        if (magican != null) 
        {
            magican.TakeHit();
        }
        Destroy(gameObject);

        FireWizard fireWizard = other.GetComponent<FireWizard>();
        if(fireWizard != null)
        {
            fireWizard.TakeHit();
        }
        Destroy(gameObject);
    }

    public void DestroySelf(AnimationEvent evt = null)
    {
        Destroy(gameObject);
    }
}
