using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/**
 * This component acts as a switch to toggle a group of lamps when activated
 * and replaces a question mark object with another prefab upon activation.
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

    [Header("Player Reference")]
    [Tooltip("Reference to the player object.")]
    [SerializeField] private Transform player;

    [Header("Question Mark Replacement")]
    [Tooltip("The question mark game object to be replaced.")]
    [SerializeField] private GameObject questionMarkObject;

    [Tooltip("The replacement prefab that will appear.")]
    [SerializeField] private GameObject replacementPrefab;

    private InputAction toggleLightAction; // Input action for toggling lights
    private bool isOn = false; // State of the switch (on/off)
    private bool isPlayerInRange = false; // Tracks if player is inside collider range

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
        toggleLightAction.performed -= TryToggleSwitch;
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

        // Replace the question mark with the new prefab
        ReplaceQuestionMark();

        // Notify the SwitchCounterManager
        if (switchCounterManager != null)
        {
            switchCounterManager.RegisterSwitchActivation(); // Update the counter
        }
    }

    /**
     * Replaces the question mark object with a prefab at the same position and rotation.
     */
    private void ReplaceQuestionMark()
    {
        if (questionMarkObject != null && replacementPrefab != null)
        {
            // Get the position and rotation of the question mark
            Vector3 position = questionMarkObject.transform.position;
            Quaternion rotation = questionMarkObject.transform.rotation;

            // Destroy the question mark object
            Destroy(questionMarkObject);

            // Instantiate the replacement prefab at the same position and rotation
            Instantiate(replacementPrefab, position, rotation);
        }
        else
        {
            Debug.LogWarning("Question Mark Object or Replacement Prefab is missing!");
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
