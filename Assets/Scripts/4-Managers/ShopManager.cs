using UnityEngine;
using TMPro;

/**
 * Manages the shop where players can purchase upgrades.
 * Integrates with CharacterKeyboardMover for movement-related upgrades.
 */
public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // Reference to the shop UI panel
    [SerializeField] private CharacterKeyboardMover playerMover; // Reference to the player's movement script
    [SerializeField] private TMP_Text currencyText; // Display player's currency

    // Prices for items
    [SerializeField] private int batteryPrice = 10;
    [SerializeField] private int heartPrice = 15;
    [SerializeField] private int speedPrice = 20;
    [SerializeField] private int jumpPrice = 25;
    [SerializeField] private int invisibilityPrice = 30;
    [SerializeField] private int unlimitedBatteryPrice = 50;

    private bool isPlayerInRange = false;
    private int playerCurrency = 100; // Example starting currency, replace with actual player currency management

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void BuyItem(string item)
    {
        switch (item)
        {
            case "Battery":
                if (playerCurrency >= batteryPrice)
                {
                    playerCurrency -= batteryPrice;
                    Debug.Log("Purchased Battery Upgrade");
                    // Add logic for battery upgrade
                }
                break;
            case "Heart":
                if (playerCurrency >= heartPrice)
                {
                    playerCurrency -= heartPrice;
                    Debug.Log("Purchased Heart Upgrade");
                    // Add logic for health increase
                }
                break;
            case "Speed":
                if (playerCurrency >= speedPrice)
                {
                    playerCurrency -= speedPrice;
                    playerMover.UpgradeSpeed(0.5f); // Example multiplier for speed
                    Debug.Log("Purchased Speed Upgrade");
                }
                break;
            case "Jump":
                if (playerCurrency >= jumpPrice)
                {
                    playerCurrency -= jumpPrice;
                    playerMover.UpgradeJump(0.5f); // Example multiplier for jump height
                    Debug.Log("Purchased Jump Upgrade");
                }
                break;
            case "Invisibility":
                if (playerCurrency >= invisibilityPrice)
                {
                    playerCurrency -= invisibilityPrice;
                    playerMover.EnableInvisibility();
                    Debug.Log("Purchased Invisibility");
                }
                break;
            case "UnlimitedBattery":
                if (playerCurrency >= unlimitedBatteryPrice)
                {
                    playerCurrency -= unlimitedBatteryPrice;
                    playerMover.EnableUnlimitedBattery();
                    Debug.Log("Purchased Unlimited Battery");
                }
                break;
            default:
                Debug.LogWarning("Invalid item");
                break;
        }

        UpdateCurrencyDisplay();
    }

    private void UpdateCurrencyDisplay()
    {
        currencyText.text = $"Currency: {playerCurrency}";
    }

    private void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        Time.timeScale = shopPanel.activeSelf ? 0 : 1; // Pause game when shop is open
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            shopPanel.SetActive(false);
            Time.timeScale = 1; // Resume game
        }
    }
}
