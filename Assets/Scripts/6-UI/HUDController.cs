using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro support

/// <summary>
/// Controls the HUD display for hearts, batteries, and coins.
/// Updates UI based on player status and score.
/// </summary>
public class HUDController : MonoBehaviour
{
    // -------------------------------
    // Serialized Fields
    // -------------------------------

    [Header("Heart Settings")]
    [SerializeField] private Image[] heartImages;          // Heart UI images
    [SerializeField] private Sprite fullHeartSprite;       // Sprite for full heart
    [SerializeField] private Sprite emptyHeartSprite;      // Sprite for empty heart

    [Header("Battery Settings")]
    [SerializeField] private Image batteryImage;           // Battery UI image
    [SerializeField] private Sprite[] batterySprites;      // Sprites for battery levels (3, 2, 1, 0)

    [Header("Coins Settings")]
    [SerializeField] private TMP_Text coinsText;           // Text for displaying coins

    // -------------------------------
    // Unity Methods
    // -------------------------------

    /// <summary>
    /// Subscribes to events and initializes the HUD at the start.
    /// </summary>
    private void Start()
    {
        // Subscribe to GameManager updates
        GameManager.Instance.onHUDUpdateCallback += UpdateHUD;

        // Subscribe to coin updates
        ScoreManager.Instance.onCoinsChangedCallback += UpdateCoins;

        // Initialize the HUD
        UpdateHUD();
        UpdateCoins(ScoreManager.Instance.GetCoins()); // Initialize coin count
    }

    /// <summary>
    /// Unsubscribes from events to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onHUDUpdateCallback -= UpdateHUD;
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.onCoinsChangedCallback -= UpdateCoins;
        }
    }

    // -------------------------------
    // HUD Updates
    // -------------------------------

    /// <summary>
    /// Updates the entire HUD (hearts and batteries).
    /// </summary>
    public void UpdateHUD()
    {
        UpdateHearts();
        UpdateBattery();
    }

    /// <summary>
    /// Updates the heart UI based on the current health.
    /// </summary>
    private void UpdateHearts()
    {
        int currentHearts = GameManager.Instance.GetCurrentHearts();

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHearts)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    /// <summary>
    /// Updates the battery UI based on the current battery level.
    /// </summary>
    private void UpdateBattery()
    {
        int currentBatteries = GameManager.Instance.GetCurrentBatteries();
        batteryImage.sprite = batterySprites[currentBatteries];
    }

    /// <summary>
    /// Updates the coins UI text.
    /// </summary>
    /// <param name="coins">The updated coin count.</param>
    private void UpdateCoins(int coins)
    {
        coinsText.text = $"{coins}"; // Displays coins properly
    }
}
