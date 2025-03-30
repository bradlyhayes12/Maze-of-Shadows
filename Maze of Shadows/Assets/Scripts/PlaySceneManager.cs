using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour {
    [Header("Room Tile Setup")]
    [SerializeField] private GameObject RoomTile;   // Assign in Inspector.
    [SerializeField] private float roomTileSize = 1f;   // Spacing for room tiles.

    private BoardManager boardManager;

    void Start(){
        // Find the BoardManager that survived DontDestroyOnLoad.
        boardManager = FindObjectOfType<BoardManager>();
        if (boardManager == null){
            Debug.LogError("No BoardManager found in PlayScene. Did you forget to load from BuildScene?");
            return;
        }
        // Wait until PlayPhase is active before spawning RoomTiles.
        StartCoroutine(WaitForActiveSceneAndSpawn());
    }

    private IEnumerator WaitForActiveSceneAndSpawn() {
        // Continuously check if the active scene is "PlayPhase".
        while (SceneManager.GetActiveScene().name != "PlayPhase")
        {
            Debug.Log("Waiting for PlayPhase to become active. Current active scene: " + SceneManager.GetActiveScene().name);
            yield return null;
        }
        Debug.Log("PlayPhase is active! Spawning RoomTiles.");
        SpawnRoomTiles();
    }

    private void SpawnRoomTiles(){
        GameObject[,] originalBoard = boardManager.board;
        int boardSize = boardManager.boardSize;

        // Calculate offset so the room tiles are centered.
        Vector3 bigBoardOffset = new Vector3(
            (boardSize - 1) * roomTileSize / 2,
            (boardSize - 1) * roomTileSize / 2,
            0f
        );

        for (int y = 0; y < boardSize; y++) {
            for (int x = 0; x < boardSize; x++) {
                GameObject tileObj = originalBoard[x, y];
                if (tileObj != null) {
                    // Calculate spawn position.
                    Vector3 spawnPos = new Vector3(x * roomTileSize, y * roomTileSize, 0f) - bigBoardOffset;
                    Debug.Log("Instantiating RoomTile at: " + spawnPos + " in scene: " + SceneManager.GetActiveScene().name);
                    
                    // Instantiate the room tile.
                    GameObject roomTile = Instantiate(RoomTile, spawnPos, Quaternion.identity);

                    // Transfer data from the original tile.
                    TileController tileController = tileObj.GetComponent<TileController>();
                    RoomTileScript roomScript = roomTile.GetComponent<RoomTileScript>();
                    roomScript.originalTileNumber = tileController.tileNumber;
                    roomScript.InitializeRoomLook();
                }
            }
        }
    }
}
