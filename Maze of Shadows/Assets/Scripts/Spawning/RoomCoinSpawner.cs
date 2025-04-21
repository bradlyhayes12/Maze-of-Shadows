using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCoinSpawner : MonoBehaviour
{
    [Tooltip("Your Coin prefab")]
    public GameObject coinPrefab;
    [Tooltip("How many coins we want (max 8 in a 3×3)")]
    public int coinsToSpawn = 8;

    // The 8 offsets around the center of a 3×3 grid
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
        // If your room's pivot is at its center, just use transform.position:
        Vector2 center = (Vector2)transform.position;

        int count = Mathf.Clamp(coinsToSpawn, 0, _offsets.Length);

        // Shuffle offsets if you want random placements
        var list = new List<Vector2>(_offsets);
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = center + list[i];
            var go = Instantiate(coinPrefab, (Vector3)spawnPos, Quaternion.identity);
            Debug.Log($"Spawned coin at {spawnPos}", go);
        }
    }
}
