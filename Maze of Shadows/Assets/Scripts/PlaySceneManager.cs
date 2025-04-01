using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [Header("Room Tile Setup")]
    [SerializeField] private GameObject RoomTile; // Assign in Inspector
    [SerializeField] private GameObject PlayerSpawnRoom;

    // 1) Hardcode the known size of each room
    [SerializeField] private float roomWidth  = 17.77157f;
    [SerializeField] private float roomHeight = 9.66798f;
    [SerializeField] private float extraGap   = 0f; // If you want a small gap, set this > 0

    private BoardManager boardManager;

    void Start()
    {
        // 2) Find BoardManager
        boardManager = FindObjectOfType<BoardManager>();
        if (boardManager == null)
        {
            Debug.LogError("No BoardManager found in PlayScene!");
            return;
        }

        // 3) (Skip tilemap measuring code — we have the numbers!)
        // Start the coroutine to wait for PlayPhase to be active
        StartCoroutine(WaitForActiveSceneAndSpawn());
    }

    private IEnumerator WaitForActiveSceneAndSpawn()
    {
        while (SceneManager.GetActiveScene().name != "PlayPhase")
        {
            Debug.Log("Waiting for PlayPhase to become active. " +
                      "Current active scene: " + SceneManager.GetActiveScene().name);
            yield return null;
        }

        Debug.Log("PlayPhase is active! Spawning RoomTiles.");
        SpawnRoomTiles();
    }

    private void SpawnRoomTiles()
    {
        GameObject[,] originalBoard = boardManager.board;
        int boardSize = boardManager.boardSize;

        Vector3 bigBoardOffset = new Vector3(
            (boardSize - 1) * roomWidth / 2f,
            (boardSize - 1) * roomHeight / 2f,
            0f
        );

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Vector3 spawnPos = new Vector3(
                    x * (roomWidth + extraGap),
                    y * (roomHeight + extraGap),
                    0f
                ) - bigBoardOffset;

                GameObject tileObj = originalBoard[x, y];

                if (tileObj != null)
                {
                    GameObject roomTile = Instantiate(RoomTile, spawnPos, Quaternion.identity);
                    
                    TileController tileController = tileObj.GetComponent<TileController>();
                    RoomTileScript roomScript = roomTile.GetComponent<RoomTileScript>();
                    roomScript.originalTileNumber = tileController.tileNumber;
                }
                else
                {
                    // This is the empty spot — spawn the PlayerSpawnRoom here!
                    Instantiate(PlayerSpawnRoom, spawnPos, Quaternion.identity);
                }
            }
        }
    }

}
