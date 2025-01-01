using UnityEngine;
using TMPro; // For TextMeshPro support

/**
 * Manages the UI for displaying the switch counter in the Canvas.
 */
public class UIManager : MonoBehaviour
{
    [Header("Counter UI")]
    [Tooltip("Text object to display the number of remaining switches.")]
    [SerializeField] private TextMeshProUGUI counterText;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one UIManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /**
     * Updates the counter display for remaining switches.
     */
    public void UpdateCounter(int remainingSwitches)
    {
        if (counterText != null)
        {
            // Update the text in the Canvas
            counterText.text = $"Switches Remaining: {remainingSwitches}";
        }
    }

    /**
     * Displays a win message when all switches are activated.
     */
    public void ShowWinMessage()
    {
        if (counterText != null)
        {
            counterText.text = "All switches activated! You Win!";
        }
    }
}
