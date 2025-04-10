using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour{
    public GameObject tilePrefab;
    public int boardSize; 
    public float tileSize = 1.0f; 

    [HideInInspector] public GameObject[,] board;
    [HideInInspector] public Vector2Int emptySpot; // Position of the empty cell
    public int moveCount = 0;

    // Store board offset for reuse
    private Vector3 boardOffset;
    private ViewManagerScript viewManagerScript;
    
    // Initialize the board when the script starts.
    public void Start() {InitializeBoard();}

    void InitializeBoard() {
        // Assuming there's an Init script in the scene with mapDimensions.
        boardSize = GameSettings.SelectedBoardSize;
        board = new GameObject[boardSize, boardSize];
        int totalTiles = boardSize * boardSize - 1;
        int number = 1;

        // Center the board at (0,0)
        boardOffset = new Vector3((boardSize - 1) * tileSize / 2, (boardSize - 1) * tileSize / 2, 0f);

        for (int y = 0; y < boardSize; y++) {
            for (int x = 0; x < boardSize; x++) {
                if (number <= totalTiles) {
                    // Adjust spawn position by subtracting boardOffset.
                    Vector3 spawnPos = new Vector3(x * tileSize, y * tileSize, 0f) - boardOffset;
                    GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                    
                    // Configure the tile.
                    TileController tile = tileObj.GetComponent<TileController>();
                    tile.boardManager = this;
                    tile.x = x;
                    tile.y = y;
                    tile.tileNumber = number;
                    tile.UpdateTileText();
                    
                    TextMeshProUGUI tileText = FindObjectOfType<TextMeshProUGUI>();
                    if (tileText != null)
                        tileText.SetText($"{number}");

                    board[x, y] = tileObj;
                    number++;
                }
                else {
                    board[x, y] = null;
                    emptySpot = new Vector2Int(x, y);
                }
            }
        }
        PrintBoardState();
    }

    public bool TryMoveTile(int tileX, int tileY) {
        if (IsAdjacent(new Vector2Int(tileX, tileY), emptySpot)) {
            MoveTile(tileX, tileY);
            return true;
        }
        return false;
    }

    bool IsAdjacent(Vector2Int a, Vector2Int b) {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy) == 1; // Only one cell apart.
    }

    void MoveTile(int tileX, int tileY) {
        GameObject tileObj = board[tileX, tileY];
        if (tileObj == null) return;

        // Swap the tile with the empty spot.
        board[emptySpot.x, emptySpot.y] = tileObj;
        board[tileX, tileY] = null;

        TileController tile = tileObj.GetComponent<TileController>();
        tile.x = emptySpot.x;
        tile.y = emptySpot.y;

        tileObj.transform.position = new Vector3(tile.x * tileSize, tile.y * tileSize, 0f) - boardOffset;

        // Update the empty spot.
        emptySpot = new Vector2Int(tileX, tileY);
        moveCount++;
        Debug.Log("Move Count: " + moveCount);
        PrintBoardState();
    }

    void PrintBoardState() {
        string state = "\nBoard State:\n";
        for (int y = boardSize - 1; y >= 0; y--){
            for (int x = 0; x < boardSize; x++){
                if (board[x, y] != null){
                    TileController tile = board[x, y].GetComponent<TileController>();
                    state += tile.tileNumber.ToString("D2") + " ";
                }
                else state += "EE ";
            }
            state += "\n";
        }
        Debug.Log(state);
    }

    public bool AreCellsAdjacent(Vector2Int a, Vector2Int b){return IsAdjacent(a, b);}

    public void OnPlayButtonClicked()
{
        // Make sure BoardManager persists.
        DontDestroyOnLoad(gameObject);
        HideVisualsAndDisableInteraction();

        viewManagerScript = FindObjectOfType<ViewManagerScript>();
        if (viewManagerScript != null){
            viewManagerScript.LoadScene("PlayPhase");
            viewManagerScript.UnloadScene("BuildPhase");
        }
        else Debug.LogError("ViewManagerScript not found.");
    }

    private void HideVisualsAndDisableInteraction(){
        // Disable this object's renderer if present.
        Renderer mainRenderer = GetComponent<Renderer>();
        if (mainRenderer != null) mainRenderer.enabled = false;

        // Disable all child renderers.
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;

        // Disable all colliders.
        foreach (Collider collider in GetComponentsInChildren<Collider>())
            collider.enabled = false;

        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
            canvas.enabled = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
