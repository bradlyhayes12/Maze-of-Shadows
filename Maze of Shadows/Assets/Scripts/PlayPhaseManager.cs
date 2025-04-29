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

        int size = Mathf.Clamp(GameSettings.SelectedBoardSize, 3, 5);
        coinsToCollect = size + 2;
    }

    void Start()
    {
        //var rooms = GameObject.FindGameObjectsWithTag("Room");

        //foreach (var ro in rooms)
        //{
        //    var bc = ro.GetComponent<Collider2D>();
        //    SpawnAllCoins(bc);
        //}
    }

    //void SpawnAllCoins(Collider2D bc)
    //{
    //    Vector2 center = bc.bounds.center;
    //}

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

#if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
        // e.g. SceneManager.LoadScene("WinScene");
        // or show a UI panel, freeze input, etc.
    }
}
