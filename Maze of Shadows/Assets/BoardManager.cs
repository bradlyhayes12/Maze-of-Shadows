using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int boardSize = 5;       // 5x5 puzzle
    public float tileSize = 1.0f;   // Each tile's spacing in world units
    private TextMeshProUGUI tileNum;

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot;   // (x,y) of the blank cell

    void Start()
    {
        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1; // For a 5x5 puzzle, 24 tiles.
        int createdTiles = 0;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (createdTiles < totalTiles)
                {
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f);
                    GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                    tileObj.transform.SetParent(transform);
                    tileNum = FindObjectOfType<TextMeshProUGUI>();

                    TileController tc = tileObj.GetComponent<TileController>();
                    tc.boardManager = this;
                    tc.x = x;
                    tc.y = y;
                    
                    tileNum.SetText($"{createdTiles}");

                    board[x, y] = tileObj;
                    createdTiles++;
                }
                else
                {
                    // Last slot is empty.
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }

        Debug.Log("=== Board Initialized ===");
        PrintBoardState();
    }

    /// <summary>
    /// Slides all tiles between the given tile position and the empty spot,
    /// moving them one step toward the empty space.
    /// </summary>
    /// <param name="tilePos">The position of the tile that was activated.</param>
    /// <param name="direction">The direction to slide (up, down, left, right).</param>
    public void SlideTiles(Vector2Int tilePos, Vector2Int direction)
    {
        // Horizontal movement
        if (direction == Vector2Int.right || direction == Vector2Int.left)
        {
            // Ensure we're in the same row.
            if (tilePos.y != emptySpot.y)
                return;

            // For right movement, the empty spot must be to the right of the tile.
            if (direction == Vector2Int.right && emptySpot.x <= tilePos.x)
                return;
            // For left movement, the empty spot must be to the left of the tile.
            if (direction == Vector2Int.left && emptySpot.x >= tilePos.x)
                return;

            // Slide horizontally.
            if (direction == Vector2Int.right)
            {
                // Shift all tiles between tilePos.x and emptySpot.x one space to the left.
                for (int col = emptySpot.x - 1; col >= tilePos.x; col--)
                {
                    board[col + 1, tilePos.y] = board[col, tilePos.y];
                    // Update tile's internal coordinate and visual position.
                    TileController tc = board[col + 1, tilePos.y].GetComponent<TileController>();
                    tc.x = col + 1;
                    board[col + 1, tilePos.y].transform.position = new Vector3((col + 1) * tileSize, tilePos.y * tileSize, 0f);
                }
            }
            else if (direction == Vector2Int.left)
            {
                // Shift all tiles between emptySpot.x and tilePos.x one space to the right.
                for (int col = emptySpot.x + 1; col <= tilePos.x; col++)
                {
                    board[col - 1, tilePos.y] = board[col, tilePos.y];
                    TileController tc = board[col - 1, tilePos.y].GetComponent<TileController>();
                    tc.x = col - 1;
                    board[col - 1, tilePos.y].transform.position = new Vector3((col - 1) * tileSize, tilePos.y * tileSize, 0f);
                }
            }
            // The original position of the activated tile now becomes empty.
            board[tilePos.x, tilePos.y] = null;
            emptySpot = tilePos;
        }
        // Vertical movement
        else if (direction == Vector2Int.up || direction == Vector2Int.down)
        {
            // Ensure we're in the same column.
            if (tilePos.x != emptySpot.x)
                return;

            // For up movement, the empty spot must be above the tile.
            if (direction == Vector2Int.up && emptySpot.y <= tilePos.y)
                return;
            // For down movement, the empty spot must be below the tile.
            if (direction == Vector2Int.down && emptySpot.y >= tilePos.y)
                return;

            // Slide vertically.
            if (direction == Vector2Int.up)
            {
                // Shift all tiles between tilePos.y and emptySpot.y one space down.
                for (int row = emptySpot.y - 1; row >= tilePos.y; row--)
                {
                    board[tilePos.x, row + 1] = board[tilePos.x, row];
                    TileController tc = board[tilePos.x, row + 1].GetComponent<TileController>();
                    tc.y = row + 1;
                    board[tilePos.x, row + 1].transform.position = new Vector3(tilePos.x * tileSize, (row + 1) * tileSize, 0f);
                }
            }
            else if (direction == Vector2Int.down)
            {
                // Shift all tiles between emptySpot.y and tilePos.y one space up.
                for (int row = emptySpot.y + 1; row <= tilePos.y; row++)
                {
                    board[tilePos.x, row - 1] = board[tilePos.x, row];
                    TileController tc = board[tilePos.x, row - 1].GetComponent<TileController>();
                    tc.y = row - 1;
                    board[tilePos.x, row - 1].transform.position = new Vector3(tilePos.x * tileSize, (row - 1) * tileSize, 0f);
                }
            }
            board[tilePos.x, tilePos.y] = null;
            emptySpot = tilePos;
        }

        Debug.Log($"[SLIDE COMPLETE] Empty spot is now at ({emptySpot.x},{emptySpot.y}).");
        PrintBoardState();
    }

    /// <summary>
    /// Prints the board state to the Console for debugging.
    /// </summary>
    public void PrintBoardState()
    {
        string boardLog = "\n\nCurrent Board State (y=0 at bottom):\n";
        for (int row = boardSize - 1; row >= 0; row--)
        {
            boardLog += "Row " + row + ": ";
            for (int col = 0; col < boardSize; col++)
            {
                boardLog += board[col, row] == null ? "[ EMPTY ] " : $"[ T({col},{row}) ] ";
            }
            boardLog += "\n";
        }
        boardLog += $"EmptySpot: ({emptySpot.x},{emptySpot.y})";
        Debug.Log(boardLog);
    }
}
