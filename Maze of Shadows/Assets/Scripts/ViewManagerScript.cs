using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManagerScript : MonoBehaviour {
    public void LoadScene(string sceneName){StartCoroutine(LoadSceneAndSetActive(sceneName));}

    private IEnumerator LoadSceneAndSetActive(string sceneName){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid()){
            SceneManager.SetActiveScene(loadedScene);
            Debug.Log($"Scene '{sceneName}' is now active!");
        }
        else
            Debug.LogError($"Scene '{sceneName}' could not be found after loading!");
    }

    public void UnloadScene(string sceneName){StartCoroutine(UnloadSceneCoroutine(sceneName));}
    
    private IEnumerator UnloadSceneCoroutine(string sceneName){
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone) yield return null;
        Debug.Log($"Scene '{sceneName}' has been unloaded.");
    }
}
