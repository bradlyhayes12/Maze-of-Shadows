using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    public int mapDimensions;
        // Start is called before the first frame update
    void Start() {
        gameObject.GetComponent<ViewManagerScript>().LoadScene("StartMenu");
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    
}
