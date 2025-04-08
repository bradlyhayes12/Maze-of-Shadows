using UnityEngine;
using TMPro;

public class TileController : MonoBehaviour
{
    [Header("References")]
    public BoardManager boardManager;

    [Header("Tile Coordinates")]
    public int x; // Grid X-coordinate
    public int y; // Grid Y-coordinate

    [Header("Tile Directions")]
    [Tooltip("e.g. 'DR', 'LD', 'LRD', etc. Must match PlaySceneManager RoomMappings exactly.")]
    public string directionString;

    [Header("UI")]
    public TextMeshProUGUI tileText;

    void Start()
    {
        // Ensure the text component is found
        if (tileText == null)
            tileText = GetComponentInChildren<TextMeshProUGUI>();

        // Hide text at the start
        if (tileText != null)
            tileText.enabled = false;
    }

    /// <summary>
    /// Shows the given text on the tile (and re-enables the text component).
    /// </summary>
    /// <param name="text">Text to display</param>
    public void UpdateTileText(string text)
    {
        if (tileText != null)
        {
            tileText.text = text;
            tileText.enabled = true;
        }
    }

    void OnMouseDown()
    {
        // Attempt to move the tile if BoardManager is available
        if (boardManager != null)
        {
            bool moved = boardManager.TryMoveTile(x, y);
            // if (!moved) Debug.Log("Tile is not adjacent to the empty spot.");
        }
    }
}
