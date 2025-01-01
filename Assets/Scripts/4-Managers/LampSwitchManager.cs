using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/**
 * This component acts as a switch to toggle a group of lamps when activated.
 */
[RequireComponent(typeof(BoxCollider))] // Requires a BoxCollider for collision
public class LampSwitchManager : MonoBehaviour
{
    [Header("Lamps Controlled")]
    [Tooltip("List of lamps controlled by the switch.")]
    [SerializeField] private List<Light> controlledLamps = new List<Light>();

    [Header("Switch Counter")]
    [Tooltip("Reference to the Switch Counter Manager.")]
    [SerializeField] private SwitchCounterManager switchCounterManager; // Counter reference.

    [Tooltip("Reference to the player object.")]
    [SerializeField] private Transform player;

    private InputAction toggleLightAction; // Input action for toggling lights
    private bool isOn = false; // State of the switch (on/off)
    private bool isPlayerInRange = false; // NEW: Tracks if player is inside collider range

    private void OnEnable()
    {
        // Create and enable input action for the "E" key
        toggleLightAction = new InputAction("ToggleSwitch", binding: "<Keyboard>/e");
        toggleLightAction.Enable();

        // Bind the action to the TryToggleSwitch method
        toggleLightAction.performed += TryToggleSwitch;
    }

    private void OnDisable()
    {
        toggleLightAction.Disable();
    }

    private void Start()
    {
        // Initialize lamps to off
        foreach (Light lamp in controlledLamps)
        {
            if (lamp != null)
            {
                lamp.enabled = false; // Start all lamps in OFF state
            }
        }

        // Ensure the BoxCollider is set correctly
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true; // Use trigger mode for proximity detection
    }

    /**
     * Toggles the lamps assigned to this specific switch.
     */
    private void TryToggleSwitch(InputAction.CallbackContext context)
    {
        if (isPlayerInRange && !isOn) // Activate only if the player is in range and the switch is off
        {
            ToggleSwitch();
        }
    }

    private void ToggleSwitch()
    {
        isOn = true;

        // Toggle only this switch's assigned lamps
        foreach (Light lamp in controlledLamps)
        {
            if (lamp != null)
            {
                lamp.enabled = true; // Turn assigned lamps ON
            }
        }

        // Notify the SwitchCounterManager
        if (switchCounterManager != null)
        {
            switchCounterManager.RegisterSwitchActivation(); // Update the counter
        }
    }

    /**
     * Detects when the player enters the collider.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player caused the collision
        {
            isPlayerInRange = true; // Player is in range
        }
    }

    /**
     * Detects when the player exits the collider.
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Player left the range
        }
    }
}
