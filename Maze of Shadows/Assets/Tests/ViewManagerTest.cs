using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;

public class ViewManagerTest
{
    private ViewManagerScript _viewManager;

    [SetUp]
    public void Setup(){
        GameObject obj = new GameObject();
        _viewManager = obj.AddComponent<ViewManagerScript>();
    }

    [Category("ViewManagerTest")]
    [UnityTest]
    public IEnumerator TestLoadScene(){
        string sceneName = "TestScene"; // Make sure this scene exists in Build Settings
        _viewManager.LoadScene(sceneName);

        // Wait until the scene is loaded
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);

        // Check if the scene is actually loaded
        Assert.IsTrue(SceneManager.GetSceneByName(sceneName).isLoaded, "Scene should be loaded");
    }

    [Category("ViewManagerTest")]
    [UnityTest]
    public IEnumerator TestUnloadScene(){
        string sceneName = "TestScene";

        // load scene
        _viewManager.LoadScene(sceneName);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);

        // unload scene
        _viewManager.UnloadScene(sceneName);
        yield return new WaitUntil(() => !SceneManager.GetSceneByName(sceneName).isLoaded);

        // check if scene is unloaded
        Assert.IsFalse(SceneManager.GetSceneByName(sceneName).isLoaded, "Scene should be unloaded");
    }
}
