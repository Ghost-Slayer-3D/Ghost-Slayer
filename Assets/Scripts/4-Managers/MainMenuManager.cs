using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.CloudSave; // For Unity Cloud Save
using System.Threading.Tasks;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button newGameButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button resumeButton;
    public InputField playerNameField;

    private string playerName;

    private void Start()
    {
        // Initialize Unity Cloud Save
        InitializeCloudSave();
        
        // Add button listeners
        newGameButton.onClick.AddListener(ShowDifficultyOptions);
        easyButton.onClick.AddListener(() => StartNewGame("Easy"));
        mediumButton.onClick.AddListener(() => StartNewGame("Medium"));
        hardButton.onClick.AddListener(() => StartNewGame("Hard"));
        resumeButton.onClick.AddListener(ResumeGame);

        // Hide difficulty options by default
        SetDifficultyOptionsActive(false);
    }

    private void InitializeCloudSave()
    {
        // Add Unity Services initialization here, if needed.
    }

    private void ShowDifficultyOptions()
    {
        SetDifficultyOptionsActive(true);
    }

    private void SetDifficultyOptionsActive(bool isActive)
    {
        easyButton.gameObject.SetActive(isActive);
        mediumButton.gameObject.SetActive(isActive);
        hardButton.gameObject.SetActive(isActive);
    }

    private async void StartNewGame(string difficulty)
    {
        playerName = playerNameField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is empty!");
            return;
        }

        // Save player data in Unity Cloud Save
        await SavePlayerData(playerName, difficulty);

        // Load the game scene
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }

    private async void ResumeGame()
    {
        playerName = playerNameField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is empty!");
            return;
        }

        // Load player data from Unity Cloud Save
        var data = await LoadPlayerData(playerName);

        if (data != null)
        {
            Debug.Log($"Loaded data for {playerName}: Difficulty - {data["Difficulty"]}");
            // Load the game scene or player progress here
            SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
        }
        else
        {
            Debug.LogWarning("No saved data found for this player!");
        }
    }

    private async Task SavePlayerData(string playerName, string difficulty)
    {
        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(new System.Collections.Generic.Dictionary<string, object>
            {
                { "PlayerName", playerName },
                { "Difficulty", difficulty }
            });

            Debug.Log("Player data saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    private async Task<System.Collections.Generic.Dictionary<string, string>> LoadPlayerData(string playerName)
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync();

            if (data.ContainsKey("PlayerName") && data["PlayerName"] == playerName)
            {
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load player data: {e.Message}");
        }

        return null;
    }
}
