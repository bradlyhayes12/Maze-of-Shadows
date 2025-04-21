using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //void Awake()
    //{
    //    Debug.Log($"[Coin] Awake on {name}");
    //}

    //void Start()
    //{
    //    var col = GetComponent<Collider2D>();
    //    Debug.Log($"[Coin] Start – Collider enabled? {col.enabled}, IsTrigger? {col.isTrigger}");
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Coin] OnTriggerEnter2D with {other.name}");
        if (other.CompareTag("Player"))
        {
            // tell manager we got colllected
            PlayPhaseManager.Instance.CollectCoin();

            Debug.Log("[Coin] It’s the player – destroying coin.");
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 5;
    }
}
