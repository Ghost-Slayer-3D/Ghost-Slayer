using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Handles the instructions panel visibility, game freezing, and multiple movement script controls.
 */
public class InstructionPanelManager : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("The instructions panel UI element.")]
    [SerializeField] private GameObject instructionsPanel;

    [Header("Movement Scripts Settings")]
    [Tooltip("Scripts controlling movement to be disabled when paused.")]
    [SerializeField] private MonoBehaviour[] movementScripts; // Array of scripts to disable/enable

    private InputAction toggleInstructionAction;
    private bool isPaused = true;

    private void OnEnable()
    {
        // Setup input action for toggling instructions
        toggleInstructionAction = new InputAction(binding: "<Keyboard>/i");
        toggleInstructionAction.Enable();
        toggleInstructionAction.performed += ToggleInstructions;
    }

    private void OnDisable()
    {
        toggleInstructionAction.Disable();
    }

    private void Start()
    {
        // Show instructions at the start and freeze the game
        ShowInstructions();
    }

    private void ToggleInstructions(InputAction.CallbackContext context)
    {
        if (instructionsPanel != null)
        {
            // Toggle visibility of the panel
            bool isActive = instructionsPanel.activeSelf;
            instructionsPanel.SetActive(!isActive);

            // Pause or resume the game
            Time.timeScale = isActive ? 1f : 0f;
            isPaused = !isActive;

            // Toggle all movement scripts in the array
            ToggleMovementScripts(isActive);
        }
    }

    private void ShowInstructions()
    {
        // Show instructions panel at the start and freeze the game
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

            // Disable all movement scripts at the start
            ToggleMovementScripts(false);
        }
    }

    private void ToggleMovementScripts(bool isEnabled)
    {
        // Iterate through all movement scripts and toggle their enabled state
        foreach (MonoBehaviour script in movementScripts)
        {
            if (script != null)
            {
                script.enabled = isEnabled;
            }
        }
    }
}
