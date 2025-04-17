using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [System.Serializable]
    public struct RoomMapping
    {
        public string buildTileDirection;
        public GameObject roomPrefab;
    }

    [Header("Room Tile Setup")]
    public RoomMapping[] roomMappings;
    [SerializeField] GameObject playerSpawnRoom;

    [SerializeField] float roomWidth  = 17.77157f;
    [SerializeField] float roomHeight =  9.66798f;
    [SerializeField] float extraGap   = 0f;

    BoardManager boardManager;
    Dictionary<string, GameObject> dir2Room;

    void Start()
    {
        dir2Room = new Dictionary<string, GameObject>();
        foreach (var m in roomMappings)
            if (!dir2Room.ContainsKey(m.buildTileDirection))
                dir2Room[m.buildTileDirection] = m.roomPrefab;

        boardManager = FindObjectOfType<BoardManager>();
        if (boardManager == null) { Debug.LogError("BoardManager missing"); return; }

        StartCoroutine(WaitForActiveSceneAndSpawn());
    }

    IEnumerator WaitForActiveSceneAndSpawn()
    {
        while (SceneManager.GetActiveScene().name != "PlayPhase")
            yield return null;

        SpawnRoomTiles();
    }

    /* ------------------------------------------------------------------ */
    void SpawnRoomTiles()
    {
        var board      = boardManager.board;
        int size       = boardManager.boardSize;
        var offset     = new Vector3((size - 1) * roomWidth * .5f,
                                     (size - 1) * roomHeight * .5f,
                                     0f);

        Vector3 spawnRoomPos = Vector3.zero;  // will hold PlayerSpawnRoom pos

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            var pos = new Vector3(x * (roomWidth + extraGap),
                                  y * (roomHeight + extraGap),
                                  0f) - offset;

            var buildTileObj = board[x, y];
            if (buildTileObj)                                  // real tile
            {
                var dir = buildTileObj.GetComponent<TileController>().directionString;
                if (dir2Room.TryGetValue(dir, out var prefab))
                    Instantiate(prefab, pos, Quaternion.identity);
            }
            else                                               // empty cell
            {
                Instantiate(playerSpawnRoom, pos, Quaternion.identity);
                spawnRoomPos = pos;                            // remember me
            }
        }

        CenterCameraOn(spawnRoomPos);
    }

    void CenterCameraOn(Vector3 target)
    {
        var cam = Camera.main;
        if (cam != null)
            cam.transform.position = new Vector3(target.x, target.y, cam.transform.position.z);
        else
            Debug.LogWarning("Main Camera not found.");
    }
    /* ------------------------------------------------------------------ */
}
 