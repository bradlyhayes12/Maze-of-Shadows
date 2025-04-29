using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiShoot : MonoBehaviour
{
    public GameObject arrowPrefab;  // assign your Arrow prefab here
    public Transform arrowSpawnPoint;  // assign your ArrowSpawnPoint here
    public float arrowSpeed = 20f;  // how fast the arrow moves

    // This function will be called from the animation event
    public void ShootArrow()
    {
        Debug.Log("ShootArrow() called!");

        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

            Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float facing = transform.localScale.x > 0 ? 1f : -1f;

                Vector2 shootDirection = new Vector2(facing, 0f); // left or right

                rb.velocity = shootDirection * arrowSpeed;

                // Flip arrow visual if facing left
                if (facing < 0)
                {
                    newArrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f); // flip for 2D
                }

                Debug.Log("Arrow launched with velocity: " + rb.velocity);
            }
            else
            {
                Debug.LogWarning("Arrow has no Rigidbody2D!");
            }
        }
    }

}
