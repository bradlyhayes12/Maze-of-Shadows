using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [System.Serializable]
    public struct RoomMapping
    {
        public string buildTileDirection;  // e.g. "DR"
        public GameObject roomPrefab;      // e.g. Room_DR
    }

    [Header("Room Tile Setup")]
    [SerializeField] private RoomMapping[] roomMappings;
    [SerializeField] private GameObject PlayerSpawnRoom;

    [SerializeField] private float roomWidth  = 17.77157f;
    [SerializeField] private float roomHeight = 9.66798f;
    [SerializeField] private float extraGap   = 0f;

    private BoardManager boardManager;
    private Dictionary<string, GameObject> directionToRoomDict;

    void Start()
    {
        // Build the dictionary for quick lookups
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

        // Find the BoardManager from the Build phase
        boardManager = FindObjectOfType<BoardManager>();
        if (boardManager == null)
        {
            Debug.LogError("No BoardManager found in PlayScene!");
            return;
        }

        StartCoroutine(WaitForActiveSceneAndSpawn());
    }

    private IEnumerator WaitForActiveSceneAndSpawn()
    {
        while (SceneManager.GetActiveScene().name != "PlayPhase")
        {
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

                GameObject buildTileObj = originalBoard[x, y];
                if (buildTileObj != null)
                {
                    TileController tileCtrl = buildTileObj.GetComponent<TileController>();
                    string direction = tileCtrl.directionString; // e.g. "DR"
                    Debug.Log($"Spawning tile at {x},{y} with direction = '{direction}'.");

                    if (directionToRoomDict.TryGetValue(direction, out GameObject roomPrefab))
                    {
                        Instantiate(roomPrefab, spawnPos, Quaternion.identity);
                    }
                    else
                    {
                        Debug.LogWarning($"No room prefab mapping found for direction '{direction}'");
                    }
                }
                else
                {
                    // Empty cell becomes the PlayerSpawnRoom
                    Instantiate(PlayerSpawnRoom, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
