using UnityEngine;

/**
 * Tracks the number of switches remaining and handles win conditions.
 */
public class SwitchCounterManager : MonoBehaviour
{
    [Header("Switch Settings")]
    [Tooltip("Total number of switches required.")]
    [SerializeField] private int totalSwitches = 0;

    private int remainingSwitches; // Count of switches left to activate

    private void Start()
    {
        // Automatically count all LampSwitchManager components in the scene
        LampSwitchManager[] switches = FindObjectsOfType<LampSwitchManager>();
        totalSwitches = switches.Length; // Count the total number of switches
        remainingSwitches = totalSwitches; // Initialize remaining count

        // Update the counter UI at the start
        UIManager.Instance.UpdateCounter(remainingSwitches);
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

            // Check if all switches are activated
            if (remainingSwitches <= 0)
            {
                OnAllSwitchesActivated(); // Trigger win condition
            }
        }
    }

    /**
     * Called when all switches are activated.
     */
    private void OnAllSwitchesActivated()
    {
        // Display win message in the UI
        UIManager.Instance.ShowWinMessage();
    }

    /**
     * Returns the number of switches remaining.
     */
    public int GetRemainingSwitches()
    {
        return remainingSwitches;
    }
}
