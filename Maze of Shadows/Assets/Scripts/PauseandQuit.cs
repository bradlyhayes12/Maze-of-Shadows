using UnityEngine;

public class PauseAndQuit : MonoBehaviour
{
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
                QuitGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Freezes the game
        isPaused = true;
        Debug.Log("Game Paused. Press Escape again to Quit.");
    }

    void QuitGame()
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
