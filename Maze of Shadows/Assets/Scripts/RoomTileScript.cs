using UnityEngine;
using TMPro;

/// <summary>
/// Represents a large "Room Tile" in the PlayScene, 
/// mapped from the small board tile in BuildScene.
/// </summary>
public class RoomTileScript : MonoBehaviour{
    public int originalTileNumber;
    
    [SerializeField] private TextMeshProUGUI roomNumberText;

    void Start(){
        if (roomNumberText == null) roomNumberText = GetComponentInChildren<TextMeshProUGUI>();
    } 

    public void InitializeRoomLook(){
        if (roomNumberText != null)
            roomNumberText.text = originalTileNumber.ToString();
    }
}
