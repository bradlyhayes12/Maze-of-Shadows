using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
}
