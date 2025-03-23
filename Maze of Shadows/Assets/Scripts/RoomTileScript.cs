using UnityEngine;
using TMPro;

/// <summary>
/// Represents a large "Room Tile" in the PlayScene, 
/// mapped from the small board tile in BuildScene.
/// </summary>
public class RoomTileScript : MonoBehaviour
{
    public int originalTileNumber;
    
    [SerializeField] private TextMeshProUGUI roomNumberText;

    // This is where you'd define your visuals or specialized data 
    // (like walls, floor textures, lighting, etc.)
    
    public void InitializeRoomLook()
    {
        // Example: show the tile number for debugging
        if (roomNumberText != null)
        {
            roomNumberText.text = "Room " + originalTileNumber.ToString();
        }

        // If you want different looks based on originalTileNumber, you could do:
        // if (originalTileNumber % 2 == 0) { /* Some style or color */ }
        // else { /* Another style */ }
    }
}
