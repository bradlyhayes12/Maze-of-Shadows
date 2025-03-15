using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int boardSize = 4;          // For a 15-puzzle (4x4)
    public float tileSize = 1.0f;        // Spacing in world units

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot; // Position of the empty cell
    public int moveCount = 0;
    private TextMeshProUGUI tileNum;

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {
        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1;
        int number = 1;

        // Create tiles and assign numbers from 1 to totalTiles.
        // The last cell is left empty.
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (number <= totalTiles)
                {
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f);
                    GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    
                    // Configure the tile.
                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;
                    tile.tileNumber = number;
                    tile.UpdateTileText();
                    tileNum = FindObjectOfType<TextMeshProUGUI>();
                    tileNum.SetText($"{number}");
                    
                    board[x, y] = tileObj;
                    number++;
                }
                else
                {
                    // Set the empty spot.
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }
        PrintBoardState();
    }

    /// <summary>
    /// Checks if the tile at (tileX, tileY) is adjacent to the empty cell.
    /// If yes, moves the tile into the empty spot.
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

    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1; // Only one cell apart
    }

    void MoveTile(int tileX, int tileY)
    {
        GameObject tileObj = board[tileX, tileY];
        if (tileObj == null) return;

        // Swap the tile with the empty spot.
        board[emptySpot.x, emptySpot.y] = tileObj;
        board[tileX, tileY] = null;

        // Update the tile's internal coordinates and its visual position.
        TileController tile = tileObj.GetComponent<TileController>();
        int oldX = tile.x, oldY = tile.y;
        tile.x = emptySpot.x;
        tile.y = emptySpot.y;
        tileObj.transform.position = new Vector3(tile.x * tileSize, tile.y * tileSize, 0f);

        // Update the empty spot to be where the tile was.
        emptySpot = new Vector2Int(tileX, tileY);
        moveCount++;
        Debug.Log("Move Count: " + moveCount);
        PrintBoardState();

        // Check for win condition.
        if (CheckWin())
        {
            Debug.Log("Puzzle solved! Total moves: " + moveCount);
            // Insert win handling code (UI, sound, etc.) here.
        }
    }

    bool CheckWin()
    {
        int number = 1;
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // Skip the empty spot (bottom-right cell in a solved puzzle).
                if (x == boardSize - 1 && y == boardSize - 1)
                    continue;

                GameObject tileObj = board[x, y];
                if (tileObj == null)
                    return false;

                TileController tile = tileObj.GetComponent<TileController>();
                if (tile.tileNumber != number)
                    return false;
                number++;
            }
        }
        return true;
    }

    void PrintBoardState()
    {
        string state = "\nBoard State:\n";
        for (int y = boardSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (board[x, y] != null)
                {
                    TileController tile = board[x, y].GetComponent<TileController>();
                    state += tile.tileNumber.ToString("D2") + " ";
                }
                else
                {
                    state += "EE ";
                }
            }
            state += "\n";
        }
        Debug.Log(state);
    }
}
