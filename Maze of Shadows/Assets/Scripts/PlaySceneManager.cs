using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    [Header("Room Tile Setup")]
    [SerializeField] private GameObject roomTilePrefab;   // Assign in Inspector
    [SerializeField] private float roomTileSize = 10f;    // Larger spacing for rooms

    private BoardManager boardManager;

    void Start()
    {
        // Find the existing BoardManager that we tagged with DontDestroyOnLoad
        boardManager = FindObjectOfType<BoardManager>();

        if (boardManager == null)
        {
            Debug.LogError("No BoardManager found in PlayScene. Did you forget to load from BuildScene?");
            return;
        }

        // We have the original small board layout. Now let's replicate them as "RoomTiles."
        SpawnRoomTiles();
    }

    private void SpawnRoomTiles()
    {
        // The original board is stored in boardManager.board
        GameObject[,] originalBoard = boardManager.board;
        int boardSize = boardManager.boardSize;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject tileObj = originalBoard[x, y];
                if (tileObj != null)
                {
                    // We have a tile; get its TileController
                    TileController tileController = tileObj.GetComponent<TileController>();

                    // Instantiate a big "RoomTile" in the PlayScene
                    Vector3 spawnPos = new Vector3(
                        x * roomTileSize, 
                        y * roomTileSize, 
                        0f
                    );

                    GameObject roomTile = Instantiate(roomTilePrefab, spawnPos, Quaternion.identity);

                    // Get our custom script
                    // RoomTile roomScript = roomTile.GetComponent<RoomTile>();
                    // if (roomScript != null)
                    // {
                    //     // Pass along the tile number to keep track
                    //     roomScript.originalTileNumber = tileController.tileNumber;
                    //     // You can also pass any "type" or "look" data if needed
                    //     roomScript.InitializeRoomLook();
                    // }
                }
            }
        }
    }
}
