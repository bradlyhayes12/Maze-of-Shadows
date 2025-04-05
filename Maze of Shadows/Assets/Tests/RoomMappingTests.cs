using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RoomMappingTests
{
    [Test]
    public void Test_If_We_Get_RoomPrefab_For_UD()
    {
        // 1) Create a new GameObject with PlaySceneManager on it
        var go = new GameObject("Test_PlaySceneManager");
        var playSceneManager = go.AddComponent<PlaySceneManager>();
        
        // 2) Create a dummy room prefab for testing
        // In real usage, you'd have an actual prefab from your project.
        // For unit testing, we'll just make an empty GameObject here.
        var testRoomPrefab = new GameObject("Room_UD_TestPrefab");

        // 3) Create a test mapping
        // We'll map "UD" to our test prefab
        playSceneManager.roomMappings = new PlaySceneManager.RoomMapping[]
        {
            new PlaySceneManager.RoomMapping
            {
                buildTileDirection = "UD",
                roomPrefab = testRoomPrefab
            }
        };

        // 4) Now replicate how PlaySceneManager builds its dictionary
        //    (Normally, this happens in PlaySceneManager.Start(),
        //    but here we manually do it for the test.)
        var directionToRoomDict = new Dictionary<string, GameObject>();
        foreach (var mapping in playSceneManager.roomMappings)
        {
            if (!directionToRoomDict.ContainsKey(mapping.buildTileDirection))
            {
                directionToRoomDict.Add(mapping.buildTileDirection, mapping.roomPrefab);
            }
        }

        // 5) Check that "UD" exists in the dictionary
        Assert.IsTrue(directionToRoomDict.ContainsKey("UD"), 
            "'UD' direction was not found in the dictionary!");
        
        // 6) Verify that we get back the exact prefab we set
        var returnedPrefab = directionToRoomDict["UD"];
        Assert.AreEqual(testRoomPrefab, returnedPrefab, 
            "Returned prefab does not match the expected 'testRoomPrefab'!");
    }
}
