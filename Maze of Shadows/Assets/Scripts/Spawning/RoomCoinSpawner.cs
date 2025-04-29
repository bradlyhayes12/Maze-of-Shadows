using System.Collections.Generic;
using UnityEngine;

public class RoomCoinSpawner : MonoBehaviour
{
    [Tooltip("Your Coin prefab")]
    public GameObject coinPrefab;

    [Header("Spawn Settings")]
    [Tooltip("Maximum number of coins that can spawn in a room")]
    [Range(0, 8)]
    public int maxCoinsToSpawn = 2;

    [Tooltip("Chance weights for 0, 1, and 2 coins (must sum to 1)")]
    public Vector3 spawnChance = new Vector3(0.2f, 0.5f, 0.3f);

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
        Vector2 center = (Vector2)transform.position;

        // 1) Decide how many coins to spawn: 0, 1, or 2
        int count = ChooseSpawnCount();

        // 2) Clamp to available slots
        count = Mathf.Clamp(count, 0, Mathf.Min(maxCoinsToSpawn, _offsets.Length));

        // 3) Shuffle possible positions
        var positions = new List<Vector2>(_offsets);
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }

        // 4) Instantiate count coins at the first count offsets
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = center + positions[i];
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }

    int ChooseSpawnCount()
    {
        // Simple weighted random between 0,1,2 using spawnChance.x/y/z
        float roll = Random.value; // 0..1
        if (roll < spawnChance.x) return 0;
        else if (roll < spawnChance.x + spawnChance.y) return 1;
        else return 2;
    }
}