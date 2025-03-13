using UnityEngine;

public class TileController : MonoBehaviour
{
    public BoardManager boardManager;
    public int x; // Board coordinate
    public int y; // Board coordinate

    void Update()
    {
        // If we're adjacent to the empty spot, allow a move
        if (IsAdjacentToEmpty())
        {
            // If the empty spot is directly above us
            if (Input.GetKeyDown(KeyCode.UpArrow) && boardManager.emptySpot.y == y + 1)
            {
                SwapWithEmpty();
            }
            // If it's directly below
            if (Input.GetKeyDown(KeyCode.DownArrow) && boardManager.emptySpot.y == y - 1)
            {
                SwapWithEmpty();
            }
            // If it's directly left
            if (Input.GetKeyDown(KeyCode.LeftArrow) && boardManager.emptySpot.x == x - 1)
            {
                SwapWithEmpty();
            }
            // If it's directly right
            if (Input.GetKeyDown(KeyCode.RightArrow) && boardManager.emptySpot.x == x + 1)
            {
                SwapWithEmpty();
            }
        }
    }

    bool IsAdjacentToEmpty()
    {
        Vector2Int e = boardManager.emptySpot;
        // Adjacent if difference in X is 1 (same Y) or difference in Y is 1 (same X)
        bool adjacentX = (Mathf.Abs(e.x - x) == 1 && e.y == y);
        bool adjacentY = (Mathf.Abs(e.y - y) == 1 && e.x == x);
        return adjacentX || adjacentY;
    }

    void SwapWithEmpty()
    {
        // 1. Store the tile’s old position
        int oldX = x;
        int oldY = y;

        // 2. Grab the empty spot
        Vector2Int e = boardManager.emptySpot;
        int emptyX = e.x;
        int emptyY = e.y;

        // 3. The tile now goes to the empty spot
        boardManager.board[emptyX, emptyY] = gameObject; 
        x = emptyX;
        y = emptyY;

        // 4. The tile’s old position becomes the new empty spot
        boardManager.board[oldX, oldY] = null;
        boardManager.emptySpot = new Vector2Int(oldX, oldY);

        // 5. Visually update the tile’s transform
        transform.position = new Vector3(x * boardManager.tileSize,
                                         y * boardManager.tileSize,
                                         0f);
    }
}
