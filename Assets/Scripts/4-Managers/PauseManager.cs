using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static string lastGameScene = null; // Tracks the last active game scene

    [Header("Input Action")]
    [SerializeField] private InputAction pauseAction;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Update()
    {
        if (pauseAction.triggered)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Debug.Log("Pausing game...");
        isPaused = true;

        // Save the current scene name to allow resuming
        lastGameScene = SceneManager.GetActiveScene().name;

        // Pause the game
        Time.timeScale = 0f;

        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Load the Main Menu additively
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming game...");
        isPaused = false;

        // Resume game time
        Time.timeScale = 1f;

        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Unload the Main Menu
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
