using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleton Instance
    public static ScoreManager Instance { get; private set; }
    private bool isDoubleCoinsActive = false; // Tracks Double Coins buff
    private int coins = 0;

    // Event for coin count updates
    public delegate void OnCoinsChanged(int newCoinCount);
    public event OnCoinsChanged onCoinsChangedCallback;

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

    public void AddCoins(int amount)
    {
        if (isDoubleCoinsActive)
        {
            amount *= 2;
            Debug.Log($"Double Coins active! Adding {amount} coins.");
        }

        coins += amount;

        // Notify UI or other systems
        onCoinsChangedCallback?.Invoke(coins);
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetDoubleCoins(bool state)
    {
        isDoubleCoinsActive = state;
        Debug.Log($"Double Coins set to {state}");
    }

    public bool IsDoubleCoinsActive()
    {
        return isDoubleCoinsActive; // Check if double coins is active
    }
}
