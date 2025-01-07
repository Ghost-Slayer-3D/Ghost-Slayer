using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro support

public class HUDController : MonoBehaviour
{
    [Header("Heart Settings")]
    public Image[] heartImages;             // Heart UI images
    public Sprite fullHeartSprite;          // Sprite for full heart
    public Sprite emptyHeartSprite;         // Sprite for empty heart

    [Header("Battery Settings")]
    public Image batteryImage;              // Battery UI image
    public Sprite[] batterySprites;         // Sprites for battery levels (3, 2, 1, 0)

    [Header("Coins Settings")]
    public TMP_Text coinsText;              // Text for coins


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

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (GameManager.Instance != null)
            GameManager.Instance.onHUDUpdateCallback -= UpdateHUD;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.onCoinsChangedCallback -= UpdateCoins;
    }

    // Updates the HUD for hearts and batteries
    public void UpdateHUD()
    {
        UpdateHearts();
        UpdateBattery();
    }

    // Update the heart UI
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

    // Update the battery UI
    private void UpdateBattery()
    {
        int currentBatteries = GameManager.Instance.GetCurrentBatteries();
        batteryImage.sprite = batterySprites[currentBatteries];
    }

    private void UpdateCoins(int coins)
    {
        coinsText.text = $"{coins}"; // Displays coins properly
    }
}
