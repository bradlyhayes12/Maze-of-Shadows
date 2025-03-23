using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    [Header("Room Tile Setup")]
    [SerializeField] private GameObject roomTilePrefab;   // Assign in Inspector
    [SerializeField] private float roomTileSize = 1f;    // Larger spacing for rooms

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

    private void SpawnRoomTiles(){
        GameObject[,] originalBoard = boardManager.board;
        int boardSize = boardManager.boardSize;

        // Calculate an offset so the "big board" is also centered at (0,0).
        Vector3 bigBoardOffset = new Vector3(
            (boardSize - 1) * roomTileSize / 2,
            (boardSize - 1) * roomTileSize / 2,
            0f
        );

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject tileObj = originalBoard[x, y];
                if (tileObj != null)
                {
                    // (x,y) -> spawn big tile, subtract offset
                    Vector3 spawnPos = new Vector3(x * roomTileSize, y * roomTileSize, 0f) - bigBoardOffset;
                    
                    GameObject roomTile = Instantiate(roomTilePrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }

}
