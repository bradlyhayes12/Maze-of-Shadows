using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // only if you want to load another scene

public class PlayPhaseManager : MonoBehaviour
{
    public static PlayPhaseManager Instance { get; private set; }

    [Header("Coin Setup")]
    public GameObject coinPrefab;         // drag your Coin prefab here
    public Transform[] coinSpawnPoints;   // drag in empty GameObjects where you want coins
    public int coinsToCollect = 4;

    private int collectedCoins = 0;

    void Awake()
    {
        // singleton so Coin can find us
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        var rooms = GameObject.FindGameObjectsWithTag("Room");

        foreach (var ro in rooms)
        {
            var bc = ro.GetComponent<BoxCollider2D>();
            SpawnAllCoins(bc);
        }
    }

    void SpawnAllCoins(BoxCollider2D bc)
    {
        Vector2 center = bc.bounds.center;
    }

    /// <summary>
    /// Called by each Coin when the player picks it up.
    /// </summary>
    public void CollectCoin()
    {
        collectedCoins++;
        Debug.Log($"Collected {collectedCoins}/{coinsToCollect} coins");

        if (collectedCoins >= coinsToCollect)
            EndGame();
    }

    void EndGame()
    {
        Debug.Log("All coins collected! Game Over!");
        // e.g. SceneManager.LoadScene("WinScene");
        // or show a UI panel, freeze input, etc.
    }
}
