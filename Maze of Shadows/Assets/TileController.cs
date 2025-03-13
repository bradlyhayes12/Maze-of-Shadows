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

            // each of the ones (1) below for getting the empty spot really just represents a single tile as the tiles count as 1x1 as the board is just 5x5 (of 1x1) tiles, and it's 1x1 because the tile prefab has its size from me just creating the square, and we put it in 5 rows 5 times, hense each tile is 1x1 in respect to the board which is in total 5x5, the 1x1 doesn't actually represent the physical property size of the tile, idk what the heck it is

            // If the empty spot is directly above us
            if (Input.GetKeyDown(KeyCode.UpArrow) && boardManager.emptySpot.y == y + 1)
                SwapWithEmpty();
            // If it's directly below
            if (Input.GetKeyDown(KeyCode.DownArrow) && boardManager.emptySpot.y == y - 1)
                SwapWithEmpty();
            // If it's directly left
            if (Input.GetKeyDown(KeyCode.LeftArrow) && boardManager.emptySpot.x == x - 1)
                SwapWithEmpty();
            // If it's directly right
            if (Input.GetKeyDown(KeyCode.RightArrow) && boardManager.emptySpot.x == x + 1)
                SwapWithEmpty();
        }
    }

    bool IsAdjacentToEmpty()
    {
        Vector2Int emptySpot = boardManager.emptySpot;
        // Adjacent if difference in X is 1 (same Y) or difference in Y is 1 (same X)
        bool adjacentX = (Mathf.Abs(emptySpot.x - x) == 1 && emptySpot.y == y);
        bool adjacentY = (Mathf.Abs(emptySpot.y - y) == 1 && emptySpot.x == x);
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
