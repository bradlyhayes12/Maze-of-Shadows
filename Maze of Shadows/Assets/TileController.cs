using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public BoardManager boardManager;
    public int x;  // Board coordinate (left->right)
    public int y;  // Board coordinate (bottom->top)

    [HideInInspector] public int tileNum;
    public Text TileNumText;

    void Update()
    {

        // Only move if adjacent to the empty spot
        if (IsAdjacentToEmpty())
        {
            // We'll do else-if so only one direction can fire per frame
            if (Input.GetKeyDown(KeyCode.UpArrow)
                && y + 1 < boardManager.boardSize
                && boardManager.emptySpot.x == x
                && boardManager.emptySpot.y == y + 1)
            {
                SwapWithEmpty();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)
                     && y - 1 >= 0
                     && boardManager.emptySpot.x == x
                     && boardManager.emptySpot.y == y - 1)
            {
                SwapWithEmpty();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)
                     && x - 1 >= 0
                     && boardManager.emptySpot.x == x - 1
                     && boardManager.emptySpot.y == y)
            {
                SwapWithEmpty();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)
                     && x + 1 < boardManager.boardSize
                     && boardManager.emptySpot.x == x + 1
                     && boardManager.emptySpot.y == y)
            {
                SwapWithEmpty();
            }
        }
    }

    bool IsAdjacentToEmpty()
    {
        Vector2Int e = boardManager.emptySpot;
        // Adjacent if x differs by 1 (same y) OR y differs by 1 (same x)
        bool adjacentX = (Mathf.Abs(e.x - x) == 1 && e.y == y);
        bool adjacentY = (Mathf.Abs(e.y - y) == 1 && e.x == x);
        return (adjacentX || adjacentY);
    }

    void SwapWithEmpty()
    {
        // 1) Store the tile’s current coords
        int oldX = x;
        int oldY = y;

        // 2) Empty spot coords
        int emptyX = boardManager.emptySpot.x;
        int emptyY = boardManager.emptySpot.y;

        Debug.Log($"[SWAP] Tile at ({oldX},{oldY}) moving into empty spot at ({emptyX},{emptyY}).");

        // 3) Put this tile in the empty spot
        boardManager.board[emptyX, emptyY] = gameObject;
        x = emptyX;
        y = emptyY;

        // 4) Old position becomes the new empty spot
        boardManager.board[oldX, oldY] = null;
        boardManager.emptySpot = new Vector2Int(oldX, oldY);

        // 5) Move tile visually
        transform.position = new Vector3(x * boardManager.tileSize,
                                         y * boardManager.tileSize,
                                         0f);

        // Log final results
        Debug.Log($"[SWAP COMPLETE] Tile is now at ({x},{y}). Empty is now at ({boardManager.emptySpot.x},{boardManager.emptySpot.y}).");
        boardManager.PrintBoardState();
    }
}
