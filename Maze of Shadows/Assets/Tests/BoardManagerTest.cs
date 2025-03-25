using NUnit.Framework; // for [test] and nunit assertions
using UnityEngine;

// no using unityengine.testtools or using system.collections because we're not doing play mode tests
[TestFixture]
public class BoardManagerTests
{
    // verifies that two horizontally adjacent cells are recognized as adjacent
    
    [Test]
    [Category("Board Manager Tests")]
    public void AreCellsAdjacent_WhenCellsAreSideBySide_ReturnsTrue(){
        // arrange: create a temp gameobject, add boardmanager
        GameObject boardObj = new GameObject("board");
        BoardManager boardManager = boardObj.AddComponent<BoardManager>();

        // act: use the public arecellsadjacent method
        bool adjacent = boardManager.AreCellsAdjacent(new Vector2Int(0, 0), new Vector2Int(1, 0));

        // assert: they should be adjacent
        Assert.IsTrue(adjacent, "cells (0,0) and (1,0) should be considered adjacent.");

        // clean up: destroy the temp object
        Object.DestroyImmediate(boardObj);
    }

    // verifies that two diagonally placed cells are not recognized as adjacent
    [Test]
    [Category("Board Manager Tests")]
    public void AreCellsAdjacent_WhenCellsAreDiagonal_ReturnsFalse(){
        // arrange
        GameObject boardObj = new GameObject("board");
        BoardManager boardManager = boardObj.AddComponent<BoardManager>();

        // act
        bool adjacent = boardManager.AreCellsAdjacent(new Vector2Int(0, 0), new Vector2Int(1, 1));

        // assert
        Assert.IsFalse(adjacent, "cells (0,0) and (1,1) should not be considered adjacent diagonally.");

        // clean up
        Object.DestroyImmediate(boardObj);
    }

    // verifies that trymovetile returns true when tile is adjacent
    [Test]
    [Category("Board Manager Tests")]
    public void TryMoveTile_WhenTileIsAdjacent_ReturnsTrue(){
        // arrange
        GameObject boardObj = new GameObject("board");
        BoardManager boardManager = boardObj.AddComponent<BoardManager>();

        // create a small 2x2 board
        boardManager.boardSize = 2;
        boardManager.board = new GameObject[2,2];

        // create a tile gameobject with a tilecontroller
        GameObject tileObj = new GameObject("tile");
        tileObj.AddComponent<TileController>();

        // place that tile in (0,1), make (1,1) the empty spot
        boardManager.board[0,1] = tileObj;
        boardManager.board[1,1] = null;
        boardManager.emptySpot = new Vector2Int(1,1);

        // act
        bool moved = boardManager.TryMoveTile(0,1);

        // assert
        Assert.IsTrue(moved, "tile at (0,1) should move into (1,1) if adjacent.");

        // clean up
        Object.DestroyImmediate(boardObj);
    }


    // verifies that trymovetile returns false when tile is not adjacent
    [Test]
    [Category("Board Manager Tests")]
    public void TryMoveTile_WhenTileIsNotAdjacent_ReturnsFalse(){
        // arrange
        GameObject boardObj = new GameObject("board");
        BoardManager boardManager = boardObj.AddComponent<BoardManager>();

        // create a small 2x2 board
        boardManager.boardSize = 2;
        boardManager.board = new GameObject[2,2];

        // place a tile in (0,0), make (1,1) the empty spot
        boardManager.board[0,0] = new GameObject("tile");
        boardManager.board[1,1] = null;
        boardManager.emptySpot = new Vector2Int(1,1);

        // act
        bool moved = boardManager.TryMoveTile(0,0);

        // assert
        Assert.IsFalse(moved, "tile at (0,0) should not move into (1,1) if not adjacent.");

        // clean up
        Object.DestroyImmediate(boardObj);
    }
}
