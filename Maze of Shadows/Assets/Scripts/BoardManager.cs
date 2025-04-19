using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject[] tilePrefabs;          // 11 prefabs

    [Header("Board Settings")]
    public int boardSize;                     // 3, 4, or 5 only
    public float tileSize = 1f;

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot;
    public int moveCount = 0;

    private Vector3 boardOffset;
    private ViewManagerScript viewManagerScript;

    /* ---------------------------  NEW  ---------------------------------- */
    /// <summary>
    /// Returns a shuffled list of exactly <paramref name="count"/> tiles,
    /// repeating prefabs as many times as necessary.
    /// </summary>
    private List<GameObject> GetShuffledTiles(int count)
    {
        // Repeat the whole prefab set until we have enough
        List<GameObject> tiles = new List<GameObject>(count);
        while (tiles.Count < count)
            tiles.AddRange(tilePrefabs);

        // Trim any extras
        tiles.RemoveRange(count, tiles.Count - count);

        // Fisher‑Yates shuffle
        System.Random rng = new System.Random();
        for (int n = tiles.Count - 1; n > 0; n--)
        {
            int k = rng.Next(n + 1);
            (tiles[n], tiles[k]) = (tiles[k], tiles[n]);
        }
        return tiles;
    }
    /* -------------------------------------------------------------------- */

    void Start() => InitializeBoard();

    void InitializeBoard()
    {
        // 1. Make sure board size is legit
        boardSize = Mathf.Clamp(GameSettings.SelectedBoardSize, 3, 5);

        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1;   // leave 1 empty

        /* 2. Grab exactly what we need, duplicates allowed */
        List<GameObject> tiles = GetShuffledTiles(totalTiles);

        int tileIndex = 0;

        // 3. Center the board
        boardOffset = new Vector3((boardSize - 1) * tileSize * 0.5f,
                                  (boardSize - 1) * tileSize * 0.5f,
                                   0f);

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (x + y * boardSize < totalTiles)       // still need tiles
                {
                    Vector3 pos = new Vector3(x * tileSize, y * tileSize, 0) - boardOffset;
                    GameObject tileObj = Instantiate(tiles[tileIndex++], pos, Quaternion.identity, transform);

                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;

                    board[x, y] = tileObj;
                }
                else                                      // last slot = empty
                {
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
    // turn off anything that draws
    foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
        r.enabled = false;

    // nuke ALL colliders (3‑D *and* 2‑D)
    foreach (Collider c in GetComponentsInChildren<Collider>(true))
        c.enabled = false;

    foreach (Collider2D c2d in GetComponentsInChildren<Collider2D>(true))
        c2d.enabled = false;          // ← NEW line

    // make the canvases disappear too
    foreach (Canvas canvas in GetComponentsInChildren<Canvas>(true))
        canvas.enabled = false;

    // optional: freeze the puzzle scripts so nothing runs in Play phase
    foreach (TileController t in GetComponentsInChildren<TileController>(true))
        t.enabled = false;
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
