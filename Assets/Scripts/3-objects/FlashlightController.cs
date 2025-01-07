using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [SerializeField] private GameObject flashlightBeam;    // Reference to the flashlight beam object
    [SerializeField] private float activeDuration = 1.5f;  // Duration the flashlight remains active
    [SerializeField] private LayerMask ghostLayer;         // Layer for detecting ghosts
    [SerializeField] private float flashlightRange = 5f;   // Range of the flashlight beam

    private bool isActive = false;                         // Tracks flashlight state
    private float activeTimer = 0f;                        // Timer for deactivation

    private PlayerInput playerInput;                       // Input system reference
    private InputAction flashlightAction;                  // Action for flashlight input

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the PlayerInput component and validate it
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on the Player GameObject!");
            return;
        }

        // Get the flashlight action from Input Actions
        flashlightAction = playerInput.actions.FindAction("Flashlight", true);

        if (flashlightAction == null)
        {
            Debug.LogError("Flashlight action is not found in Input Actions!");
        }
    }

    // Enable the input action when the object is enabled
    private void OnEnable()
    {
        if (flashlightAction != null)
        {
            flashlightAction.performed += ActivateFlashlight;
        }
    }

    // Disable the input action when the object is disabled
    private void OnDisable()
    {
        if (flashlightAction != null)
        {
            flashlightAction.performed -= ActivateFlashlight;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            activeTimer -= Time.deltaTime;

            // Deactivate flashlight if timer runs out
            if (activeTimer <= 0f)
            {
                DeactivateFlashlight();
            }
        }
    }

    // Activates the flashlight
    private void ActivateFlashlight(InputAction.CallbackContext context)
    {
        // Check if the flashlight is already active or if there are no batteries
        if (isActive || GameManager.Instance.GetCurrentBatteries() <= 0)
            return;

        // Activate the flashlight beam and start the timer
        flashlightBeam.SetActive(true);
        isActive = true;
        activeTimer = activeDuration;

        // Reduce battery count
        GameManager.Instance.UseBattery();

        // Check for ghosts in range and destroy them
        KillGhosts();
    }

    // Deactivates the flashlight
    private void DeactivateFlashlight()
    {
        flashlightBeam.SetActive(false);
        isActive = false;
    }

    // Detects and damages ghosts within range
    private void KillGhosts()
    {
        // Use RaycastAll to detect ghosts within the flashlight's range
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, flashlightRange, ghostLayer);

        foreach (var hit in hits)
        {
            GhostController ghost = hit.collider.GetComponent<GhostController>();
            if (ghost != null)
            {
                ghost.TakeDamage(); // Damage or destroy the ghost
            }
        }
    }

    // Draws the flashlight range in the scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * flashlightRange);
    }
}
