using UnityEngine;
using TMPro;
public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int boardSize; // reference the board size from the view 
    public float tileSize = 1.0f; // Spacing in world units

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot; // Position of the empty cell
    public int moveCount = 0;
    private TextMeshProUGUI tileNum;

    private Init init;

    // Store board offset for reuse
    private Vector3 boardOffset;

    public void Start() {InitializeBoard();}

    void InitializeBoard() {
        boardSize = FindObjectOfType<Init>().mapDimensions; // Get board dimensions from your view
        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1;
        int number = 1;

        // Calculate board offset so that the board's center is at (0,0)
        boardOffset = new Vector3((boardSize - 1) * tileSize / 2, (boardSize - 1) * tileSize / 2, 0f);

        for (int y = 0; y < boardSize; y++) {
            for (int x = 0; x < boardSize; x++) {
                if (number <= totalTiles) {
                    // Adjust spawn position by subtracting boardOffset
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f) - boardOffset;
                    GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    
                    // Configure the tile.
                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;
                    tile.tileNumber = number;
                    tile.UpdateTileText();
                    
                    // Set the tile number on a TextMeshProUGUI component.
                    TextMeshProUGUI tileNum = FindObjectOfType<TextMeshProUGUI>();
                    tileNum.SetText($"{number}");

                    board[x, y] = tileObj;
                    number++;
                }
                else {
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
    public bool TryMoveTile(int tileX, int tileY) {
        if (IsAdjacent(new Vector2Int(tileX, tileY), emptySpot)) {
            MoveTile(tileX, tileY);
            return true;
        }
        return false;
    }

    bool IsAdjacent(Vector2Int a, Vector2Int b) {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1; // Only one cell apart
    }

    void MoveTile(int tileX, int tileY) {
        GameObject tileObj = board[tileX, tileY];
        if (tileObj == null) return;

        // Swap the tile with the empty spot.
        board[emptySpot.x, emptySpot.y] = tileObj;
        board[tileX, tileY] = null;

        // Update the tile's internal coordinates and its visual position.
        TileController tile = tileObj.GetComponent<TileController>();
        tile.x = emptySpot.x;
        tile.y = emptySpot.y;

        // Apply the same board offset when moving the tile
        tileObj.transform.position = new Vector3(tile.x * tileSize, tile.y * tileSize, 0f) - boardOffset;

        // Update the empty spot to be where the tile was.
        emptySpot = new Vector2Int(tileX, tileY);
        moveCount++;
        Debug.Log("Move Count: " + moveCount);
        PrintBoardState();
    }

    void PrintBoardState(){
        string state = "\nBoard State:\n";
        for (int y = boardSize - 1; y >= 0; y--){
            for (int x = 0; x < boardSize; x++){
                if (board[x, y] != null){
                    TileController tile = board[x, y].GetComponent<TileController>();
                    state += tile.tileNumber.ToString("D2") + " ";
                } 
                else state += "EE ";
            }
            state += "\n";
        }
        Debug.Log(state);
    }

    // unit testing function
    public bool AreCellsAdjacent(Vector2Int a, Vector2Int b){return IsAdjacent(a, b);}

}