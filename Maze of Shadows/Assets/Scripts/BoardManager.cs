using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject[] tilePrefabs; // Should contain 11 "Tile_XYZ" prefabs

    [Header("Board Settings")]
    public int boardSize;          // e.g. 5 for a 5x5 board
    public float tileSize = 1.0f;  // Physical size of each tile

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot; // Position of the empty cell
    public int moveCount = 0;                      // Tracks how many times a tile has been moved

    private Vector3 boardOffset;                   // Center offset for spawning the board
    private ViewManagerScript viewManagerScript;

    void Start()
    {
        InitializeBoard();
    }

    /// <summary>
    /// Randomly choose N unique tiles from a pool of prefabs.
    /// </summary>
    private GameObject[] ChooseRandomTiles(GameObject[] pool, int count)
    {
        if (pool.Length < count)
        {
            Debug.LogWarning("Not enough tile prefabs to fulfill unique selection. " +
                             $"Pool size: {pool.Length}, requested: {count}");
            count = pool.Length; // fallback so we don't exceed the array
        }

        // Convert to List to shuffle easily
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

        // Return the first 'count' shuffled items
        return list.GetRange(0, count).ToArray();
    }

    /// <summary>
    /// Spawns the board tiles, leaving one cell empty.
    /// </summary>
    void InitializeBoard()
    {
        // If you use an Init script with mapDimensions, you can retrieve boardSize from it
        boardSize = FindObjectOfType<Init>().mapDimensions;

        board = new GameObject[boardSize, boardSize];

        // The total number of tiles to place (one cell is empty).
        int totalTiles = boardSize * boardSize - 1;

        // Randomly choose 'totalTiles' distinct tile prefabs from the tilePrefabs array
        GameObject[] chosenTiles = ChooseRandomTiles(tilePrefabs, totalTiles);

        // For laying tiles in the grid
        int tileIndex = 0;
        int number = 1;

        // Calculate board offset so the board is centered at (0,0)
        boardOffset = new Vector3(
            (boardSize - 1) * tileSize / 2,
            (boardSize - 1) * tileSize / 2,
            0f
        );

        // Loop through the grid and place tiles or leave empty
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // As long as we haven't placed all of our tiles yet
                if (number <= totalTiles)
                {
                    // Position
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f) - boardOffset;

                    // Spawn a randomly chosen tile from our selected group
                    GameObject tileObj = Instantiate(chosenTiles[tileIndex], spawnPos, Quaternion.identity, transform);

                    // Configure tile settings
                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;
                    // tile.directionString is already set in the prefab

                    board[x, y] = tileObj;

                    tileIndex++;
                    number++;
                }
                else
                {
                    // The last cell is empty
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Attempts to move a tile if it is adjacent to the empty spot.
    /// </summary>
    public bool TryMoveTile(int tileX, int tileY)
    {
        if (IsAdjacent(new Vector2Int(tileX, tileY), emptySpot))
        {
            MoveTile(tileX, tileY);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if two cells are next to each other (Manhattan distance == 1).
    /// </summary>
    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1;
    }

    /// <summary>
    /// Moves the tile at (tileX, tileY) into the empty spot.
    /// </summary>
    void MoveTile(int tileX, int tileY)
    {
        GameObject tileObj = board[tileX, tileY];
        if (tileObj == null) return;

        // Swap tile with the empty spot
        board[emptySpot.x, emptySpot.y] = tileObj;
        board[tileX, tileY] = null;

        // Update tile's x, y to the empty spot
        TileController tile = tileObj.GetComponent<TileController>();
        tile.x = emptySpot.x;
        tile.y = emptySpot.y;

        // Move the tile visually
        tileObj.transform.position = new Vector3(tile.x * tileSize, tile.y * tileSize, 0f) - boardOffset;

        // Update the empty spot to the old tile position
        emptySpot = new Vector2Int(tileX, tileY);

        moveCount++;
        Debug.Log("Move Count: " + moveCount);
    }

    /// <summary>
    /// Public helper to check adjacency for other scripts.
    /// </summary>
    public bool AreCellsAdjacent(Vector2Int a, Vector2Int b)
    {
        return IsAdjacent(a, b);
    }

    /// <summary>
    /// Called when you press "Play" to transition from BuildPhase to PlayPhase.
    /// </summary>
    public void OnPlayButtonClicked()
    {
        // Keep BoardManager alive across scene loads
        DontDestroyOnLoad(gameObject);

        // Hide visuals and disable colliders
        HideVisualsAndDisableInteraction();

        // Switch scenes via ViewManagerScript
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

    /// <summary>
    /// Hides all visuals and disables interactions for the entire board (and children).
    /// </summary>
    private void HideVisualsAndDisableInteraction()
    {
        // If this object has a renderer, disable it
        Renderer mainRenderer = GetComponent<Renderer>();
        if (mainRenderer != null)
            mainRenderer.enabled = false;

        // Disable all child renderers
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;

        // Disable all colliders
        foreach (Collider collider in GetComponentsInChildren<Collider>())
            collider.enabled = false;

        // Disable all canvases (text, etc.)
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
            canvas.enabled = false;
    }
}
