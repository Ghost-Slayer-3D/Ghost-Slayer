using UnityEngine;
using System.Collections;

/**
 * Tracks the number of switches remaining and handles win conditions.
 */
public class SwitchCounterManager : MonoBehaviour
{
    [Header("Switch Settings")]
    [Tooltip("Total number of switches required.")]
    [SerializeField] private int totalSwitches = 0;

    private int remainingSwitches; // Count of switches left to activate

    [Header("UI Settings")]
    [Tooltip("Text object from the canvas to display messages.")]
    [SerializeField] private GameObject switchMessageCanvas;

    [Tooltip("Duration to display the switch activation message (in seconds).")]
    [SerializeField] private float messageDisplayDuration = 2.0f;

    private void Start()
    {
        // Automatically count all LampSwitchManager components in the scene
        LampSwitchManager[] switches = FindObjectsOfType<LampSwitchManager>();
        totalSwitches = switches.Length; // Count the total number of switches
        remainingSwitches = totalSwitches; // Initialize remaining count

        // Update the counter UI at the start
        UIManager.Instance.UpdateCounter(remainingSwitches);

        // Ensure the canvas text is hidden at the start
        if (switchMessageCanvas != null)
        {
            switchMessageCanvas.SetActive(false);
        }
    }

    /**
     * Decrements the switch count when a switch is activated.
     */
    public void RegisterSwitchActivation()
    {
        if (remainingSwitches > 0)
        {
            remainingSwitches--;

            // Update the UI counter
            UIManager.Instance.UpdateCounter(remainingSwitches);

            // Display the canvas message
            if (switchMessageCanvas != null)
            {
                StartCoroutine(DisplaySwitchMessage());
            }

            // Check if all switches are activated
            if (remainingSwitches <= 0)
            {
                OnAllSwitchesActivated(); // Trigger win condition
            }
        }
    }

    private IEnumerator DisplaySwitchMessage()
    {
        switchMessageCanvas.SetActive(true); // Show the message
        ScoreManager.Instance.AddCoins(2); // Add 2 coins to the player
        yield return new WaitForSeconds(messageDisplayDuration); // Wait for the specified duration
        switchMessageCanvas.SetActive(false); // Hide the message
    }

    private void OnAllSwitchesActivated()
    {
        Debug.Log("All switches activated!");

        // Notify the GameManager about the win condition
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleGameOutcome(true); // Player won
        }
        else
        {
            Debug.LogWarning("GameManager instance not found. Cannot handle win condition.");
        }
    }

    /**
     * Returns the number of switches remaining.
     */
    public int GetRemainingSwitches()
    {
        return remainingSwitches;
    }
}
