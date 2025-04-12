using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSizeButton : MonoBehaviour
{
    public int size;

    public void SetBoardSize()
    {
        GameSettings.SelectedBoardSize = size;
        Debug.Log("Selected board size: " + size);
    }
}
