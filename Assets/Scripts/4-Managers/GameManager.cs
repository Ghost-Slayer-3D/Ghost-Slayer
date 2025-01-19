using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private int currentHearts;

    [Header("Battery Settings")]
    [SerializeField] private int maxBatteries = 3;
    [SerializeField] private int currentBatteries;

    [Header("Player Buffs")]
    private bool isPlayerInvisible = false;
    private bool isUnlimitedHP = false; // Tracks Unlimited HP buff
    private bool isUnlimitedBattery = false; // Tracks Unlimited Battery buff
    private const string MainMenuMessageKey = "MainMenuMessage";

    // HUD Update Event
    public delegate void OnHUDUpdate();
    public event OnHUDUpdate onHUDUpdateCallback;

    // Awake is called when the script instance is loaded
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Ensure persistence between scenes
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize health and batteries
        currentHearts = maxHearts;
        currentBatteries = maxBatteries;

        // Notify HUD to update at the start
        onHUDUpdateCallback?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (isUnlimitedHP)
        {
            Debug.Log("Unlimited HP active! Ignoring damage.");
            return;
        }

        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();

        // Trigger Game Over if health reaches zero
        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();
    }

    public void UseBattery()
    {
        if (currentBatteries > 0)
        {
            currentBatteries--;
            currentBatteries = Mathf.Clamp(currentBatteries, 0, maxBatteries);

            // Notify HUD
            onHUDUpdateCallback?.Invoke();
        }
    }

    public void AddBattery(int amount)
    {
        currentBatteries += amount;
        currentBatteries = Mathf.Clamp(currentBatteries, 0, maxBatteries);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    public int GetCurrentBatteries()
    {
        return currentBatteries;
    }

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public int GetMaxBatteries()
    {
        return maxBatteries;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");

        // Notify the MainMenu with a lose message
        HandleGameOutcome(false); // Player lost
    }

    public void SetPlayerInvisible(bool state)
    {
        isPlayerInvisible = state;
        Debug.Log($"Player invisibility set to {state}");

        // Notify all enemies and ghosts about invisibility change
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.OnPlayerInvisibilityChanged(state);
        }

        GhostController[] ghosts = FindObjectsOfType<GhostController>();
        foreach (var ghost in ghosts)
        {
            ghost.OnPlayerInvisibilityChanged(state);
        }
    }

    public bool IsPlayerInvisible()
    {
        return isPlayerInvisible;
    }

    public void SetUnlimitedHP(bool state)
    {
        isUnlimitedHP = state;
        Debug.Log($"Unlimited HP set to {state}");
    }

    public bool IsUnlimitedHP()
    {
        return isUnlimitedHP;
    }

    public void SetUnlimitedBattery(bool state)
    {
        isUnlimitedBattery = state;
        Debug.Log($"Unlimited Battery set to {state}");
    }

    public bool IsUnlimitedBattery()
    {
        return isUnlimitedBattery;
    }

    public void HandleGameOutcome(bool isWin)
    {
        string message = isWin ? "You won! Play again?" : "You Lost! Try Again?";

        // Save the message for the MainMenu
        PlayerPrefs.SetString("MainMenuMessage", message);
        PlayerPrefs.Save();

        Debug.Log(message);

        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
