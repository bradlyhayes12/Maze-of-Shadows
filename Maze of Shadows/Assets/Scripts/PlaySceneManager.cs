using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [System.Serializable]
    public struct RoomMapping
    {
        public string buildTileDirection;  // e.g. "UD"
        public GameObject roomPrefab;      // e.g. "Room_UD" prefab
    }

    [Header("Room Tile Setup")]
    [SerializeField] private RoomMapping[] roomMappings;
    [SerializeField] private GameObject PlayerSpawnRoom;

    // 1) Hardcode the known size of each room
    [SerializeField] private float roomWidth  = 17.77157f;
    [SerializeField] private float roomHeight = 9.66798f;
    [SerializeField] private float extraGap   = 0f;

    private BoardManager boardManager;

    // build this dictionary at runtime so we can quickly look up the correct room prefab
    private Dictionary<string, GameObject> directionToRoomDict;

    void Start()
    {
        // Build our dictionary from the array
        directionToRoomDict = new Dictionary<string, GameObject>();
        foreach (RoomMapping mapping in roomMappings)
        {
            if (!directionToRoomDict.ContainsKey(mapping.buildTileDirection))
            {
                directionToRoomDict.Add(mapping.buildTileDirection, mapping.roomPrefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate direction key found: {mapping.buildTileDirection}. " +
                                 "Make sure each direction is unique!");
            }
        }

        // 2) Find BoardManager
        boardManager = FindObjectOfType<BoardManager>();
        if (boardManager == null)
        {
            Debug.LogError("No BoardManager found in PlayScene!");
            return;
        }

        // Start the coroutine to wait for PlayPhase to be active, then spawn rooms
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
        // Grab our 2D array of build tiles from BoardManager
        GameObject[,] originalBoard = boardManager.board;
        int boardSize = boardManager.boardSize;

        // This offset centers the grid of rooms in scene
        Vector3 bigBoardOffset = new Vector3(
            (boardSize - 1) * roomWidth / 2f,
            (boardSize - 1) * roomHeight / 2f,
            0f
        );

        // Traverse the board
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // Calculate where this room goes in world space
                Vector3 spawnPos = new Vector3(
                    x * (roomWidth + extraGap),
                    y * (roomHeight + extraGap),
                    0f
                ) - bigBoardOffset;

                // This is the tile from the build phase; could be null if it's the empty cell
                GameObject buildTileObj = originalBoard[x, y];

                if (buildTileObj != null)
                {
                    // 1) Grab the direction string off the build tile
                    TileController tileCtrl = buildTileObj.GetComponent<TileController>();
                    string direction = tileCtrl.directionString;  // e.g. "UD"

                    // 2) Look up the matching room prefab from our dictionary
                    if (directionToRoomDict.TryGetValue(direction, out GameObject roomPrefab))
                    {
                        // 3) Instantiate that exact room
                        GameObject roomTile = Instantiate(roomPrefab, spawnPos, Quaternion.identity);

                        // Optional: if need references to a script on the room:
                        // RoomTileScript roomScript = roomTile.GetComponent<RoomTileScript>();
                        // roomScript.originalTileNumber = tileCtrl.tileNumber;
                    }
                    else
                    {
                        // If no mapping found, log warning or use fallback
                        Debug.LogWarning($"No room prefab mapping found for direction '{direction}'");
                    }
                }
                else
                {
                    // This is the empty cell — place the PlayerSpawnRoom
                    Instantiate(PlayerSpawnRoom, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
