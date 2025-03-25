using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonScript : MonoBehaviour
{
    //   Start is called before the first frame update
    private ViewManagerScript viewManager; // component is viewManager

    void Start()
    {
        viewManager = FindObjectOfType<ViewManagerScript>(); // looking it up by class name (the main class name in its script), you're searching for a GameObject that has a ViewManager component attached. Once it finds it, it returns back a reference to the script (component) named ViewManager

        // all of this is possible because this current scene that the button is in, was loaded in additively, which means both scenes (the start game and the managers scene) are running at the same time, allowing for unity to search gameobjects in both to find the script in the other active scene (managers)
    }
    public void startGame(){
        // Debug.Log("running start game from button");
        viewManager.UnloadScene("StartMenu");
        viewManager.LoadScene("BuildPhase");
    }

}
