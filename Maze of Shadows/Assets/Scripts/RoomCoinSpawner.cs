using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomCoinSpawner : MonoBehaviour
{
    [Tooltip("Your Coin prefab")]
    public GameObject coinPrefab;
    [Tooltip("How many coins we want (max 8 in a 3×3)")]
    public int coinsToSpawn = 8;

    // If your room is exactly 3×3 tiles of size 1 unit, these are the 8 offsets around the center:
    private static readonly Vector2[] _offsets = new Vector2[]
    {
        new Vector2(-1, +1), new Vector2(0, +1), new Vector2(+1, +1),
        new Vector2(-1,  0),                   /*skip center*/   new Vector2(+1,  0),
        new Vector2(-1, -1), new Vector2(0, -1), new Vector2(+1, -1),
    };

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        // Calculate world‐space center of the room from its BoxCollider2D
        var bc = GetComponent<Collider2D>();
        Vector2 center = bc.bounds.center;

        // We only want up to 8 coins, so cap it:
        int count = Mathf.Clamp(coinsToSpawn, 0, _offsets.Length);

        // If you want them in random positions instead of every slot, shuffle offsets:
        var list = new List<Vector2>(_offsets);
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        // Spawn your coins at center + offset
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = center + list[i];
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
