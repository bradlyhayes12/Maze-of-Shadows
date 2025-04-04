using UnityEngine;
using TMPro;

/// <summary>
/// Represents a large "Room Tile" in the PlayScene, 
/// mapped from the small board tile in BuildScene.
/// </summary>
public class RoomTileScript : MonoBehaviour{
    public int originalTileNumber;
    
    [SerializeField] private TextMeshProUGUI roomNumberText;
    [SerializeField] private GameObject whiteBG;
    [SerializeField] private GameObject canvas;

    void Start(){
        if (roomNumberText == null) roomNumberText = GetComponentInChildren<TextMeshProUGUI>();
        // Disable all non-tilemap stuff
        Transform bg = transform.Find("whiteBG");
        if (bg != null) bg.gameObject.SetActive(false);

        Transform canvas = transform.Find("Canvas");
        if (canvas != null) canvas.gameObject.SetActive(false);
    } 

    public void InitializeRoomLook(){
        if (roomNumberText != null)
            roomNumberText.text = originalTileNumber.ToString();
    }
}
