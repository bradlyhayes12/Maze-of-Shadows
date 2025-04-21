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

    /* ────────── Inspector Fields ────────── */
    [Header("Room Tile Setup")]
    public RoomMapping[] roomMappings;
    [SerializeField] private GameObject playerSpawnRoom;
    
    [Header("Border Tile")]
    [SerializeField] private GameObject borderTilePrefab;   // ← NEW

    [SerializeField] private float roomWidth  = 17.77157f;
    [SerializeField] private float roomHeight =  9.66798f;
    [SerializeField] private float extraGap   = 0f;

    /* ────────── Internals ────────── */
    private BoardManager boardManager;
    private Dictionary<string, GameObject> dir2Room;

    /* ────────── Life‑cycle ────────── */
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

    /* ────────── Generation ────────── */
    void SpawnRoomTiles()
    {
        var board      = boardManager.board;
        int size       = boardManager.boardSize;
        var cellSize   = new Vector3(roomWidth + extraGap, roomHeight + extraGap, 0f);
        var offset     = new Vector3((size - 1) * cellSize.x * 0.5f,
                                     (size - 1) * cellSize.y * 0.5f,
                                     0f);

        Vector3 spawnRoomPos = Vector3.zero;

        // 1) Lay down the maze itself
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            var pos = new Vector3(x * cellSize.x, y * cellSize.y, 0f) - offset;

            var buildTileObj = board[x, y];
            if (buildTileObj)                                       // occupied
            {
                var dir = buildTileObj.GetComponent<TileController>().directionString;
                if (dir2Room.TryGetValue(dir, out var prefab))
                    Instantiate(prefab, pos, Quaternion.identity);
            }
            else                                                    // player start
            {
                var spawnedRoom = Instantiate(playerSpawnRoom, pos, Quaternion.identity);
                spawnRoomPos = pos;

                // 🔍 Try to find a Camera inside the spawned room prefab
                //Camera roomCam = spawnedRoom.GetComponentInChildren<Camera>();

                // if (roomCam != null)
                // {
                //     // Disable all other cameras
                //     foreach (Camera cam in Camera.allCameras)
                //     {
                //         if (cam != roomCam)
                //         {
                //             cam.enabled = false;
                //             Debug.Log("❌ Disabled camera: " + cam.name);
                //         }
                //     }

                //     // Enable the spawn room camera
                //     roomCam.enabled = true;
                //     Debug.Log("✅ Activated spawn room camera: " + roomCam.name);
                // }
                // else
                // {
                //     Debug.LogWarning("⚠️ No camera found inside player spawn room prefab.");
                // }

                            }
                        }

                        // 2) Ring the maze with border tiles  ❏❏❏❏❏
                        SpawnBorderRing(size, cellSize, offset);

                        // 3) Center the camera on the player’s spawn
                        CenterCameraOn(spawnRoomPos);
                        Camera mainCam = Camera.main;
                        if (mainCam != null)
                        {
                            var zoomScript = mainCam.GetComponent<CameraZoomIn>();
                            if (zoomScript != null)
                            {
                                zoomScript.StartZoom(spawnRoomPos);
                                Debug.Log(" Started cinematic zoom into spawn room.");
                            }
                            else
                            {
                                Debug.LogWarning("Main Camera is missing CameraZoomIn script.");
                            }
                        }
                        
                    }
    //dont need anymore
    void DisableAllExceptMainCamera()
    {
        Camera[] allCams = Camera.allCameras;
        Camera mainCam = Camera.main;

        Debug.Log("Disabling extra cameras...");

        foreach (Camera cam in allCams)
        {
            if (cam != null)
            {
                bool shouldBeEnabled = cam == mainCam;
                cam.enabled = shouldBeEnabled;

                Debug.Log((shouldBeEnabled ? " Keeping" : " Disabling") + $" camera: {cam.name} (Position: {cam.transform.position})");
            }
        }
    }


    void SpawnBorderRing(int size, Vector3 cellSize, Vector3 offset)
    {
        if (borderTilePrefab == null) { Debug.LogWarning("BorderTile prefab not assigned."); return; }

        for (int y = -1; y <= size; y++)
        for (int x = -1; x <= size; x++)
        {
            bool isBorderCell = (x == -1 || x == size || y == -1 || y == size);
            if (!isBorderCell) continue;

            var pos = new Vector3(x * cellSize.x, y * cellSize.y, 0f) - offset;
            Instantiate(borderTilePrefab, pos, Quaternion.identity);
        }
    }

    /* ────────── Helpers ────────── */
    void CenterCameraOn(Vector3 target)
    {
        var cam = Camera.main;
        if (cam != null)
            cam.transform.position = new Vector3(target.x, target.y, cam.transform.position.z);
        else
            Debug.LogWarning("Main Camera not found.");
    }
}
