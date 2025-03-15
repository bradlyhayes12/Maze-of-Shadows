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
        int totalTiles = boardSize * boardSize - 1; // 24 in a 5x5
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
                    // The last slot is empty
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }

        Debug.Log("=== Board Initialized ===");
        PrintBoardState();
    }

    /// <summary>
    /// Prints the entire board state to the Console, row by row.
    /// </summary>
    public void PrintBoardState()
    {
        string boardLog = "\n\nCurrent Board State (y=0 at bottom):\n";
        // We'll print from the top row down (boardSize - 1) to 0
        for (int row = boardSize - 1; row >= 0; row--)
        {
            boardLog += "Row " + row + ": ";
            for (int col = 0; col < boardSize; col++)
            {
                if (board[col, row] == null)
                {
                    boardLog += "[ EMPTY ] ";
                }
                else
                {
                    // If you want to show tile name or coords, you could do:
                    boardLog += "[ T(" + col + "," + row + ") ] ";
                }
            }
            boardLog += "\n";
        }
        boardLog += "EmptySpot: (" + emptySpot.x + "," + emptySpot.y + ")";
        Debug.Log(boardLog);
    }
}
