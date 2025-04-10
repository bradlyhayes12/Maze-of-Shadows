using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject[] tilePrefabs; // Should contain your 11 tile prefabs

    [Header("Board Settings")]
    public int boardSize;          // e.g. 5 for a 5x5 board
    public float tileSize = 1.0f;

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot;
    public int moveCount = 0;

    private Vector3 boardOffset;
    private ViewManagerScript viewManagerScript;

    void Start()
    {
        InitializeBoard();
    }

    private GameObject[] ChooseRandomTiles(GameObject[] pool, int count)
    {
        if (pool.Length < count)
        {
            Debug.LogWarning("Not enough tile prefabs to fulfill unique selection. " +
                             $"Pool size: {pool.Length}, requested: {count}");
            count = pool.Length;
        }

        List<GameObject> list = new List<GameObject>(pool);

        // Fisher-Yates shuffle
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

        return list.GetRange(0, count).ToArray();
    }

    void InitializeBoard() {
        // Assuming there's an Init script in the scene with mapDimensions.
        boardSize = GameSettings.SelectedBoardSize;
        board = new GameObject[boardSize, boardSize];

        int totalTiles = boardSize * boardSize - 1; // e.g. 24 for a 5x5

        // Randomly choose 'totalTiles' from tilePrefabs (or fewer if not enough)
        GameObject[] chosenTiles = ChooseRandomTiles(tilePrefabs, totalTiles);

        int tileIndex = 0;
        int number = 1;

        // Offset so board is centered at (0,0)
        boardOffset = new Vector3(
            (boardSize - 1) * tileSize / 2,
            (boardSize - 1) * tileSize / 2,
            0f
        );

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (number <= totalTiles)
                {
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f) - boardOffset;

                    GameObject tileObj = Instantiate(chosenTiles[tileIndex], spawnPos, Quaternion.identity, transform);

                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;

                    board[x, y] = tileObj;

                    tileIndex++;
                    number++;
                }
                else
                {
                    // This is the empty cell
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }
    }

    public bool TryMoveTile(int tileX, int tileY)
    {
        if (IsAdjacent(new Vector2Int(tileX, tileY), emptySpot))
        {
            MoveTile(tileX, tileY);
            return true;
        }
        return false;
    }

    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1;
    }

    void MoveTile(int tileX, int tileY)
    {
        GameObject tileObj = board[tileX, tileY];
        if (tileObj == null) return;

        board[emptySpot.x, emptySpot.y] = tileObj;
        board[tileX, tileY] = null;

        TileController tile = tileObj.GetComponent<TileController>();
        tile.x = emptySpot.x;
        tile.y = emptySpot.y;

        tileObj.transform.position = new Vector3(tile.x * tileSize, tile.y * tileSize, 0f) - boardOffset;

        emptySpot = new Vector2Int(tileX, tileY);

        moveCount++;
        Debug.Log("Move Count: " + moveCount);
    }

    public bool AreCellsAdjacent(Vector2Int a, Vector2Int b)
    {
        return IsAdjacent(a, b);
    }

    public void OnPlayButtonClicked()
    {
        DontDestroyOnLoad(gameObject);
        HideVisualsAndDisableInteraction();

        viewManagerScript = FindObjectOfType<ViewManagerScript>();
        if (viewManagerScript != null)
        {
            viewManagerScript.LoadScene("PlayPhase");
            viewManagerScript.UnloadScene("BuildPhase");
        }
        else
        {
            Debug.LogError("ViewManagerScript not found.");
        }
    }

    private void HideVisualsAndDisableInteraction()
    {
        Renderer mainRenderer = GetComponent<Renderer>();
        if (mainRenderer != null)
            mainRenderer.enabled = false;

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;

        foreach (Collider collider in GetComponentsInChildren<Collider>())
            collider.enabled = false;

        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
            canvas.enabled = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
