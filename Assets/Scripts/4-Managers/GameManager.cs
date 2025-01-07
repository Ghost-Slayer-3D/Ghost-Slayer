using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHearts = 3;
    [SerializeField] private int currentHearts;

    [Header("Battery Settings")]
    [SerializeField] private int maxBatteries = 3;
    [SerializeField] private int currentBatteries;
    
    public delegate void OnHUDUpdate();
    public event OnHUDUpdate onHUDUpdateCallback;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize health and batteries
        currentHearts = maxHearts;
        currentBatteries = maxBatteries;

        // Notify HUD to update on start
        onHUDUpdateCallback?.Invoke();
    }

    // Health Management
    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        // Notify HUD
        onHUDUpdateCallback?.Invoke();

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

    // Battery Management
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

    // Getters for HUD
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    public int GetCurrentBatteries()
    {
        return currentBatteries;
    }

    // Game Over Logic
    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");
    }

    // Getters for pickups
    public int GetMaxHearts()
    {
        return maxHearts;
    }

    // Getters for pickups
    public int GetMaxBatteries()
    {
        return maxBatteries;
    }
}
