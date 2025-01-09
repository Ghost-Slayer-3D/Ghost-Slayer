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
    public GameObject instructionsPanel;
    public Button closeInstructionsButton;

    private PauseManager pauseManager;

    private void Start()
    {
        // Reference the PauseManager in the game scene
        pauseManager = FindObjectOfType<PauseManager>();

        // Add button listeners
        resumeButton.onClick.AddListener(() => pauseManager?.ResumeGame());
        easyButton.onClick.AddListener(() => StartNewGame("Easy"));
        mediumButton.onClick.AddListener(() => StartNewGame("Medium"));
        hardButton.onClick.AddListener(() => StartNewGame("Hard"));
        instructionsButton.onClick.AddListener(ShowInstructions);
        quitButton.onClick.AddListener(QuitGame);

        closeInstructionsButton.onClick.AddListener(HideInstructions);

        // Update Resume button state
        UpdateResumeButton();

        // Hide instructions panel initially
        instructionsPanel.SetActive(false);
    }

    private void UpdateResumeButton()
    {
        // Enable or disable the Resume button based on the game state
        bool canResume = !string.IsNullOrEmpty(PauseManager.lastGameScene);
        resumeButton.interactable = canResume;
    }

    private void StartNewGame(string difficulty)
    {
        Debug.Log($"Starting new game with difficulty: {difficulty}");
        PauseManager.lastGameScene = difficulty; // Save the new game scene name
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(difficulty); // Load the difficulty scene
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
