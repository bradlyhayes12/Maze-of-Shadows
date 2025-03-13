using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // Needed for Coroutines

public class TileController : MonoBehaviour
{
    public BoardManager boardManager;
    public int x;  // Board coordinate (left->right)
    public int y;  // Board coordinate (bottom->top)

    [HideInInspector] public int tileNum;
    public Text TileNumText;

    // Controls whether we can move and how often
    private bool canMove = true;
    public float moveCooldown = 0.05f; // Adjust in Inspector as needed

    void Update()
    {
        // If we can't move yet, skip
        if (!canMove) return;

        // Only move if adjacent to the empty spot
        if (IsAdjacentToEmpty())
        {
            // We'll do else-if so only one direction can fire per frame
            if (Input.GetKeyDown(KeyCode.UpArrow)
                && y + 1 < boardManager.boardSize
                && boardManager.emptySpot.x == x
                && boardManager.emptySpot.y == y + 1)
            {
                StartCoroutine(DoMove());
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)
                     && y - 1 >= 0
                     && boardManager.emptySpot.x == x
                     && boardManager.emptySpot.y == y - 1)
            {
                StartCoroutine(DoMove());
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)
                     && x - 1 >= 0
                     && boardManager.emptySpot.x == x - 1
                     && boardManager.emptySpot.y == y)
            {
                StartCoroutine(DoMove());
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)
                     && x + 1 < boardManager.boardSize
                     && boardManager.emptySpot.x == x + 1
                     && boardManager.emptySpot.y == y)
            {
                StartCoroutine(DoMove());
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

    private IEnumerator DoMove()
    {
        // Temporarily lock movement
        canMove = false;

        // Perform the tile swap
        SwapWithEmpty();

        // Brief pause to prevent rapid-fire swapping
        yield return new WaitForSeconds(moveCooldown);

        // Re-enable movement
        canMove = true;
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
