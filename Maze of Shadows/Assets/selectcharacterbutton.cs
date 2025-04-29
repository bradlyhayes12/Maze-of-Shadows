using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    [Tooltip("Drag in all your character prefabs here")]
    public GameObject[] characterPrefabs;

    // index of the one the player picked
    private int selectedIndex = 0;

    void Awake()
    {
        // singleton so it survives scene load
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Called by each UI button to select a character.
    /// </summary>
    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < characterPrefabs.Length)
            selectedIndex = index;
        else
            Debug.LogWarning($"CharacterSelection: invalid index {index}");
    }

    /// <summary>
    /// Use this in your Play scene to get the chosen prefab.
    /// </summary>
    public GameObject GetSelectedCharacterPrefab()
    {
        return characterPrefabs[selectedIndex];
    }

    /// <summary>
    /// Call this from your “Start Game” button.
    /// </summary>
    public void StartGame(string playSceneName)
    {
        SceneManager.LoadScene(playSceneName);
    }
}
