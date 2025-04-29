using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CoinTotalSpawnBMTests 
{
    [UnityTest]
    public IEnumerator TotalCoinsSpawnCapcity_IsAtLeast_CoinsNeeded()
    {
        yield return SceneManager.LoadSceneAsync("BuildPhase", LoadSceneMode.Single);
        yield return null;

        var bm = Object.FindObjectOfType<BoardManager>();
        Assert.IsNotNull(bm, "BoardManager not found in BuildPhase scene.");

        Object.DontDestroyOnLoad(bm.gameObject);

        yield return SceneManager.LoadSceneAsync("PlayPhase", LoadSceneMode.Single);
        yield return null;

        yield return null;

        RoomCoinSpawner[] spawners = Object.FindObjectsOfType<RoomCoinSpawner>();
        int roomCount = spawners.Length;

        int size = bm.boardSize;
        int coinsNeeded = size + 2;
        int capacity = roomCount * 2;

        Assert.GreaterOrEqual(
            capacity,
            coinsNeeded,
             $" Board {size}×{size}: only capacity={capacity} but need coinsNeeded={coinsNeeded}"
             );

        SceneManager.UnloadSceneAsync("PlayPhase");
        yield return null;
    }
}
