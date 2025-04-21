using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class RoomMappingTests
{
    [Test]
    public void Test_GetRoomPrefabForUD()
    {
        // create a new playscenemanager gameobject
        var go = new GameObject("Test_PlaySceneManager");
        var playSceneManager = go.AddComponent<PlaySceneManager>();

        // create a dummy room prefab just for testing
        var testRoomPrefab = new GameObject("Room_UD_TestPrefab");

        // set up a single mapping: "UD" -> testRoomPrefab
        playSceneManager.roomMappings = new PlaySceneManager.RoomMapping[]
        {
            new PlaySceneManager.RoomMapping
            {
                buildTileDirection = "UD",
                roomPrefab = testRoomPrefab
            }
        };

        // mimic how the dictionary is built in PlaySceneManager
        var directionToRoomDict = new Dictionary<string, GameObject>();
        foreach (var mapping in playSceneManager.roomMappings)
        {
            if (!directionToRoomDict.ContainsKey(mapping.buildTileDirection))
            {
                directionToRoomDict.Add(mapping.buildTileDirection, mapping.roomPrefab);
            }
        }

        // check that "UD" is in the dictionary
        Assert.IsTrue(directionToRoomDict.ContainsKey("UD"), 
            "'UD' direction was not found in the dictionary");

        // verify it returns the correct prefab
        var returnedPrefab = directionToRoomDict["UD"];
        Assert.AreEqual(testRoomPrefab, returnedPrefab, 
            "returned prefab does not match our testRoomPrefab");
    }

    [Test]
    public void Test_GetRoomPrefabForMultipleDirections()
    {
        // create a new playscenemanager gameobject
        var go = new GameObject("Test_PlaySceneManager");
        var playSceneManager = go.AddComponent<PlaySceneManager>();

        // create dummy prefabs
        var prefabUD = new GameObject("Room_UD_TestPrefab");
        var prefabLR = new GameObject("Room_LR_TestPrefab");
        var prefabDR = new GameObject("Room_DR_TestPrefab");

        // set up multiple mappings
        playSceneManager.roomMappings = new PlaySceneManager.RoomMapping[]
        {
            new PlaySceneManager.RoomMapping { buildTileDirection = "UD", roomPrefab = prefabUD },
            new PlaySceneManager.RoomMapping { buildTileDirection = "LR", roomPrefab = prefabLR },
            new PlaySceneManager.RoomMapping { buildTileDirection = "DR", roomPrefab = prefabDR }
        };

        // build the dictionary
        var directionToRoomDict = new Dictionary<string, GameObject>();
        foreach (var mapping in playSceneManager.roomMappings)
        {
            if (!directionToRoomDict.ContainsKey(mapping.buildTileDirection))
            {
                directionToRoomDict.Add(mapping.buildTileDirection, mapping.roomPrefab);
            }
        }

        // make sure ud, lr, dr all exist
        Assert.IsTrue(directionToRoomDict.ContainsKey("UD"), "dictionary should have 'UD'");
        Assert.IsTrue(directionToRoomDict.ContainsKey("LR"), "dictionary should have 'LR'");
        Assert.IsTrue(directionToRoomDict.ContainsKey("DR"), "dictionary should have 'DR'");

        // verify each maps to the correct prefab
        Assert.AreEqual(prefabUD, directionToRoomDict["UD"], "wrong prefab for 'UD'");
        Assert.AreEqual(prefabLR, directionToRoomDict["LR"], "wrong prefab for 'LR'");
        Assert.AreEqual(prefabDR, directionToRoomDict["DR"], "wrong prefab for 'DR'");
    }

    [Test]
    public void Test_MissingDirection()
    {
        // create a new playscenemanager gameobject
        var go = new GameObject("Test_PlaySceneManager");
        var playSceneManager = go.AddComponent<PlaySceneManager>();

        var prefabUD = new GameObject("Room_UD_TestPrefab");
        playSceneManager.roomMappings = new PlaySceneManager.RoomMapping[]
        {
            new PlaySceneManager.RoomMapping { buildTileDirection = "UD", roomPrefab = prefabUD }
        };

        // build the dictionary
        var directionToRoomDict = new Dictionary<string, GameObject>();
        foreach (var mapping in playSceneManager.roomMappings)
        {
            if (!directionToRoomDict.ContainsKey(mapping.buildTileDirection))
            {
                directionToRoomDict.Add(mapping.buildTileDirection, mapping.roomPrefab);
            }
        }

        // confirm that "DR" is not mapped
        Assert.IsFalse(directionToRoomDict.ContainsKey("DR"),
            "directionToRoomDict should not contain 'DR'");

    }
    private BoardManager BuildBoard(int size)
    {
        var go = new GameObject($"BoardManager_{size}");
        var bm = go.AddComponent<BoardManager>();

        /* -- feed a dummy prefab so GetShuffledTiles() is happy -- */
        var dummyTile               = new GameObject("DummyTile");
        dummyTile.AddComponent<TileController>().directionString = string.Empty;
        bm.tilePrefabs              = new[] { dummyTile };

        /* ---------- tell GameSettings which board size we want ---------- */
        typeof(GameSettings)                               
            .GetField("SelectedBoardSize",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .SetValue(null, size);                 

        /* -------------- now build the board normally -------------------- */
        typeof(BoardManager)
            .GetMethod("InitializeBoard", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(bm, null);

        return bm;
    }
    /* ───────────────── 4 × 4 sanity ───────────────── */
    [Test]
    public void Board4x4_HasCorrectDimensionsAndOneSpawn()
    {
        var bm = BuildBoard(4);

        Assert.IsNotNull(bm.board);
        Assert.AreEqual(4, bm.board.GetLength(0));
        Assert.AreEqual(4, bm.board.GetLength(1));

        int empty = 0;
        foreach (var cell in bm.board) if (cell == null) empty++;
        Assert.AreEqual(1, empty, "Expected exactly one empty spawn cell on 4×4 board");
    }

    /* ───────────────── 5 × 5 sanity ───────────────── */
    [Test]
    public void Board5x5_HasCorrectDimensionsAndOneSpawn()
    {
        var bm = BuildBoard(5);

        Assert.AreEqual(5, bm.board.GetLength(0));
        Assert.AreEqual(5, bm.board.GetLength(1));

        int empty = 0;
        foreach (var cell in bm.board) if (cell == null) empty++;
        Assert.AreEqual(1, empty, "Expected exactly one empty spawn cell on 5×5 board");
    }

    /* ───────────── Edge‑door check on 5 × 5 ───────────── */
    [Test]
    public void Board5x5_EdgeTilesHaveNoOutwardDoors()
    {
        var bm = BuildBoard(5);
        int n  = bm.boardSize;

        for (int x = 0; x < n; x++)
        {
            // skip if this edge tile is the empty spawn
            if (bm.board[x, n - 1] != null)
                Assert.IsFalse(bm.board[x, n - 1]
                            .GetComponent<TileController>().directionString.Contains("U"),
                            $"Top edge tile ({x},{n-1}) has an up‑facing door");

            if (bm.board[x, 0] != null)
                Assert.IsFalse(bm.board[x, 0]
                            .GetComponent<TileController>().directionString.Contains("D"),
                            $"Bottom edge tile ({x},0) has a down‑facing door");
        }

        for (int y = 0; y < n; y++)
        {
            if (bm.board[0, y] != null)
                Assert.IsFalse(bm.board[0, y]
                            .GetComponent<TileController>().directionString.Contains("L"),
                            $"Left edge tile (0,{y}) has a left‑facing door");

            if (bm.board[n - 1, y] != null)
                Assert.IsFalse(bm.board[n - 1, y]
                            .GetComponent<TileController>().directionString.Contains("R"),
                            $"Right edge tile ({n-1},{y}) has a right‑facing door");
        }
    }
}
