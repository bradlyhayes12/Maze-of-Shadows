using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int boardSize = 5;       // 5x5
    public float tileSize = 1.0f;   // How large each tile is in Unity units

    [HideInInspector] public GameObject[,] board;    // 2D array of all tiles
    [HideInInspector] public Vector2Int emptySpot;   // The (x,y) of our empty tile

    void Start()
    {
        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1; // 24
        int createdTiles = 0;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (createdTiles < totalTiles)
                {
                    // Create a tile at (x, y) in world space
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f);
                    GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity); // create instance of the tile prefab
                    tileObj.transform.SetParent(transform); // set the parent of the tile to the buildboard gameobject 

                    // Set up the tile's script
                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;

                    board[x, y] = tileObj;
                    createdTiles++;
                }
                else
                {
                    // The final spot is empty
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }
    }
}
