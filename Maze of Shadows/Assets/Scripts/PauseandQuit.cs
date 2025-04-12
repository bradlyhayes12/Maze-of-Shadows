using UnityEngine;

public class PauseAndQuit : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Freezes the game
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Debug.Log("Game Paused. Press Escape again to Quit.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Debug.Log("Game Resumed.");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
