using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button resumeButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button instructionsButton;
    public Button quitButton;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel; // Reference to the panel
    public Button closeInstructionsButton; // Reference to the close button

    [Header("Pause Menu")]
    public GameObject mainMenuPanel; // Reference to the main menu panel (can be shown in-game)

    private bool isGameRunning = false;

    private void Start()
    {
        // Add button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        easyButton.onClick.AddListener(() => StartNewGame("Easy"));
        mediumButton.onClick.AddListener(() => StartNewGame("Medium"));
        hardButton.onClick.AddListener(() => StartNewGame("Hard"));
        instructionsButton.onClick.AddListener(ShowInstructions);
        quitButton.onClick.AddListener(QuitGame);

        closeInstructionsButton.onClick.AddListener(HideInstructions);

        // Ensure menus are in the correct state
        instructionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        UpdateResumeButton();
    }

    private void Update()
    {
        // Listen for ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        bool isPaused = mainMenuPanel.activeSelf;
        mainMenuPanel.SetActive(!isPaused);

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void UpdateResumeButton()
    {
        // Enable or disable the Resume button based on game state
        resumeButton.interactable = isGameRunning;
    }

    private void StartNewGame(string difficulty)
    {
        Debug.Log($"Starting new game with difficulty: {difficulty}");
        isGameRunning = true;
        UpdateResumeButton();
        SceneManager.LoadScene(difficulty); // Load the scene corresponding to the difficulty
    }

    private void ResumeGame()
    {
        Debug.Log("Resuming game...");
        if (isGameRunning)
        {
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f; // Resume game time
        }
    }

    private void PauseGame()
    {
        Debug.Log("Pausing game...");
        Time.timeScale = 0f; // Pause game time
    }

    private void ShowInstructions()
    {
        Debug.Log("Showing instructions...");
        instructionsPanel.SetActive(true);
    }

    private void HideInstructions()
    {
        Debug.Log("Hiding instructions...");
        instructionsPanel.SetActive(false);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
