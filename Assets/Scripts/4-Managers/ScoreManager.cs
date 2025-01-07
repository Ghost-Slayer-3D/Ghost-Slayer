using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int coins = 0;

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
        coins += amount;

        // Notify UI or other systems
        onCoinsChangedCallback?.Invoke(coins);
    }

    public int GetCoins()
    {
        return coins;
    }
}
