using UnityEngine;

/// <summary>
/// Manages the player's score (coins) and notifies other systems of changes.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // Singleton Instance
    public static ScoreManager Instance { get; private set; }

    private int coins = 0;

    // Event for coin count updates
    public delegate void OnCoinsChanged(int newCoinCount);
    public event OnCoinsChanged onCoinsChangedCallback;

    /// <summary>
    /// Ensures only one instance of the ScoreManager exists.
    /// </summary>
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
    }

    /// <summary>
    /// Adds coins and notifies listeners of the updated coin count.
    /// </summary>
    /// <param name="amount">The number of coins to add.</param>
    public void AddCoins(int amount)
    {
        coins += amount;

        // Notify UI or other systems
        onCoinsChangedCallback?.Invoke(coins);
    }

    /// <summary>
    /// Returns the current number of coins.
    /// </summary>
    /// <returns>Current coin count.</returns>
    public int GetCoins()
    {
        return coins;
    }
}
