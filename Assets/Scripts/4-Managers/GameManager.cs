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

    // -------------------------------
    // Health Management
    // -------------------------------

    /// <summary>
    /// Reduces the player's health.
    /// </summary>
    /// <param name="amount">Amount of damage.</param>
    public void TakeDamage(int amount)
    {
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

    /// <summary>
    /// Heals the player's health.
    /// </summary>
    /// <param name="amount">Amount to heal.</param>
    public void Heal(int amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();
    }

    // -------------------------------
    // Battery Management
    // -------------------------------

    /// <summary>
    /// Reduces battery count by 1.
    /// </summary>
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

    /// <summary>
    /// Adds batteries up to the maximum limit.
    /// </summary>
    /// <param name="amount">Number of batteries to add.</param>
    public void AddBattery(int amount)
    {
        currentBatteries += amount;
        currentBatteries = Mathf.Clamp(currentBatteries, 0, maxBatteries);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();
    }

    // -------------------------------
    // Getters for HUD
    // -------------------------------

    /// <summary>
    /// Gets the current number of hearts.
    /// </summary>
    /// <returns>Current health.</returns>
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    /// <summary>
    /// Gets the current number of batteries.
    /// </summary>
    /// <returns>Current battery count.</returns>
    public int GetCurrentBatteries()
    {
        return currentBatteries;
    }

    // -------------------------------
    // Getters for Max Values
    // -------------------------------

    /// <summary>
    /// Gets the maximum number of hearts.
    /// </summary>
    /// <returns>Maximum health.</returns>
    public int GetMaxHearts()
    {
        return maxHearts;
    }

    /// <summary>
    /// Gets the maximum number of batteries.
    /// </summary>
    /// <returns>Maximum battery count.</returns>
    public int GetMaxBatteries()
    {
        return maxBatteries;
    }

    // -------------------------------
    // Game Over Logic
    // -------------------------------

    /// <summary>
    /// Handles the Game Over scenario.
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");
    }
}
