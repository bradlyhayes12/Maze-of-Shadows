using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    Collider2D col;
    HashSet<Collider2D> hitThisSwing = new HashSet<Collider2D>();

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;  // only enabled during the active frames of your attack
    }

    // Call this from your animation event or attack controller
    public void EnableHitbox()
    {
        hitThisSwing.Clear();
        col.enabled = true;
        Debug.Log("Hitbox ENABLED");
    }

    // Call this when your attack animation’s active window ends
    public void DisableHitbox()
    {
        col.enabled = false;
        Debug.Log("Hitbox DISABLED");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1) must be enabled
        if (!col.enabled) return;

        // 2) only hit players
        if (!other.CompareTag("Enemy")) return;

        // 3) and only hit each one once per swing
        if (hitThisSwing.Contains(other)) return;
        hitThisSwing.Add(other);

        var dmgReciver = GetComponent<SamurailHealth>();
        if (dmgReciver != null)
        {
            dmgReciver.TakeHit();
        }
      
    }
}
