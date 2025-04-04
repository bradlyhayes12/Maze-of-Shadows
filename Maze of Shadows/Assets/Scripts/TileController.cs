using UnityEngine;
using TMPro;

public class TileController : MonoBehaviour
{
    public BoardManager boardManager;
    public int x;         // Current grid x-coordinate
    public int y;         // Current grid y-coordinate
    // public int tileNumber;
    public TextMeshProUGUI tileText;

    void Start() {
        // Ensure the text component is set.
        if (tileText == null) tileText = GetComponentInChildren<TextMeshProUGUI>();

        // Disable the text on start so it's not visible
        if (tileText != null) tileText.enabled = false;
    }


    /// <summary>
    /// Update the displayed number on the tile.
    /// </summary>
    public void UpdateTileText(string text) {
        if (tileText != null) {
            tileText.text = text;
            tileText.enabled = true;
        }
    }


    void OnMouseDown() {
        // Debug.Log("Tile " + tileNumber + " clicked.");
        if (boardManager != null) {
            bool moved = boardManager.TryMoveTile(x, y);
            // if (!moved)
            //     Debug.Log("Tile " + tileNumber + " is not adjacent to the empty spot.");
        }
    }

}
