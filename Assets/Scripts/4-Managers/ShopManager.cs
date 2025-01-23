using UnityEngine;
using UnityEngine.UI; // For RawImage

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // Reference to the shop UI panel
    [SerializeField] private CharacterKeyboardMover playerMover; // Reference for movement-related upgrades
    [SerializeField] private InputRotator playerCameraRotator; // Reference to the camera rotation script

    [SerializeField] private int batteryPrice = 5;
    [SerializeField] private int heartPrice = 5;
    [SerializeField] private int speedPrice = 5;
    [SerializeField] private int jumpPrice = 5;
    [SerializeField] private int invisibilityPrice = 5;
    [SerializeField] private int unlimitedBatteryPrice = 5;
    [SerializeField] private int unlimitedHpPrice = 5;
    [SerializeField] private int doubleCoinsPrice = 5;

    [SerializeField] private RawImage invisibilityIndicator;
    [SerializeField] private RawImage unlimitedBatteryIndicator;
    [SerializeField] private RawImage unlimitedHpIndicator;
    [SerializeField] private RawImage doubleCoinsIndicator;
    [SerializeField] private RawImage jumpIndicator;
    [SerializeField] private RawImage speedIndicator;

    private bool isPlayerInRange = false;
    private bool speedPurchased = false;
    private bool jumpPurchased = false;

    private void Start()
    {
        shopPanel.SetActive(false);

        invisibilityIndicator.gameObject.SetActive(false);
        unlimitedBatteryIndicator.gameObject.SetActive(false);
        unlimitedHpIndicator.gameObject.SetActive(false);
        doubleCoinsIndicator.gameObject.SetActive(false);
        jumpIndicator.gameObject.SetActive(false);
        speedIndicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void BuyItem(string item)
    {
        Debug.Log($"Attempting to buy: {item}");

        switch (item)
        {
            case "Battery":
                if (GameManager.Instance.GetCurrentBatteries() < GameManager.Instance.GetMaxBatteries())
                {
                    if (TryPurchase(batteryPrice, "Battery"))
                    {
                        GameManager.Instance.AddBattery(1);
                        Debug.Log("Bought 1 Battery");
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot buy Battery: Already at max capacity.");
                }
                break;

            case "Heart":
                if (GameManager.Instance.GetCurrentHearts() < GameManager.Instance.GetMaxHearts())
                {
                    if (TryPurchase(heartPrice, "Heart"))
                    {
                        GameManager.Instance.Heal(1);
                        Debug.Log("Bought 1 Heart");
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot buy Heart: Already at max health.");
                }
                break;

            case "Speed":
                if (!speedPurchased)
                {
                    if (TryPurchase(speedPrice, "Speed"))
                    {
                        playerMover.UpgradeSpeed(0.5f);
                        speedIndicator.gameObject.SetActive(true);
                        speedPurchased = true;
                        Debug.Log("Bought Speed Upgrade");
                    }
                }
                else
                {
                    Debug.LogWarning("Speed Upgrade already purchased.");
                }
                break;

            case "Jump":
                if (!jumpPurchased)
                {
                    if (TryPurchase(jumpPrice, "Jump"))
                    {
                        playerMover.UpgradeJump(0.5f);
                        jumpIndicator.gameObject.SetActive(true);
                        jumpPurchased = true;
                        Debug.Log("Bought Jump Upgrade");
                    }
                }
                else
                {
                    Debug.LogWarning("Jump Upgrade already purchased.");
                }
                break;

            case "Invisibility":
                if (!GameManager.Instance.IsPlayerInvisible())
                {
                    if (TryPurchase(invisibilityPrice, "Invisibility"))
                    {
                        playerMover.EnableInvisibility(120f); // Enable invisibility for 120 seconds
                        invisibilityIndicator.gameObject.SetActive(true); // Show the indicator
                        Debug.Log("Bought Invisibility Buff (2 minutes)");
                    }
                }
                else
                {
                    Debug.LogWarning("Invisibility Buff already purchased.");
                }
                break;

            case "UnlimitedBattery":
                Debug.Log("Checking Unlimited Battery purchase conditions.");
                if (!GameManager.Instance.IsUnlimitedBattery())
                {
                    Debug.Log("Unlimited Battery is not active.");
                    if (TryPurchase(unlimitedBatteryPrice, "Unlimited Battery"))
                    {
                        playerMover.EnableUnlimitedBattery();
                        GameManager.Instance.AddBattery(3);
                        GameManager.Instance.SetUnlimitedBattery(true);
                        unlimitedBatteryIndicator.gameObject.SetActive(true); // Show the indicator
                        Debug.Log("Bought Unlimited Battery Buff");
                    }
                    else
                    {
                        Debug.LogWarning("Not enough currency to purchase Unlimited Battery!");
                    }
                }
                else
                {
                    Debug.LogWarning("Unlimited Battery Buff already purchased.");
                }
                break;

            case "UnlimitedHP":
                Debug.Log("Checking Unlimited HP purchase conditions.");
                if (!GameManager.Instance.IsUnlimitedHP())
                {
                    Debug.Log("Unlimited HP is not active.");
                    if (TryPurchase(unlimitedHpPrice, "Unlimited HP"))
                    {
                        GameManager.Instance.SetUnlimitedHP(true);
                        unlimitedHpIndicator.gameObject.SetActive(true); // Show the indicator
                        Debug.Log("Bought Unlimited HP Buff");
                    }
                    else
                    {
                        Debug.LogWarning("Not enough currency to purchase Unlimited HP!");
                    }
                }
                else
                {
                    Debug.LogWarning("Unlimited HP Buff already purchased.");
                }
                break;

            case "DoubleCoins":
                Debug.Log("Checking Double Coins purchase conditions.");
                if (!ScoreManager.Instance.IsDoubleCoinsActive())
                {
                    Debug.Log("Double Coins is not active.");
                    if (TryPurchase(doubleCoinsPrice, "Double Coins"))
                    {
                        ScoreManager.Instance.SetDoubleCoins(true);
                        doubleCoinsIndicator.gameObject.SetActive(true); // Show the indicator
                        Debug.Log("Bought Double Coins Buff");
                    }
                    else
                    {
                        Debug.LogWarning("Not enough currency to purchase Double Coins!");
                    }
                }
                else
                {
                    Debug.LogWarning("Double Coins Buff already purchased.");
                }
                break;

            default:
                Debug.LogWarning("Invalid item name: " + item);
                break;
        }
    }

    private bool TryPurchase(int price, string itemName)
    {
        if (ScoreManager.Instance.GetCoins() >= price)
        {
            ScoreManager.Instance.AddCoins(-price);
            Debug.Log($"Successfully purchased {itemName}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Not enough currency to purchase {itemName}!");
            return false;
        }
    }

    private void ToggleShop()
    {
        bool shopIsOpen = !shopPanel.activeSelf;
        shopPanel.SetActive(shopIsOpen);
        Time.timeScale = shopIsOpen ? 0 : 1;

        Cursor.visible = shopIsOpen;
        Cursor.lockState = shopIsOpen ? CursorLockMode.None : CursorLockMode.Locked;

        if (playerCameraRotator != null)
        {
            playerCameraRotator.enabled = !shopIsOpen;
        }
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
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (playerCameraRotator != null)
            {
                playerCameraRotator.enabled = true;
            }
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCameraRotator != null)
        {
            playerCameraRotator.enabled = true;
        }
    }
}
