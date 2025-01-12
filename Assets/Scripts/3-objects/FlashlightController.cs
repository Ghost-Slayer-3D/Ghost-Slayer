using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/**
 * Controls the flashlight, including visual light intensity and instant ghost elimination.
 */
public class FlashlightController : MonoBehaviour
{   
    [Header("Drop Items")]
    [Tooltip("Prefab for the heart item.")]
    [SerializeField] private GameObject heartPrefab;

    [Tooltip("Prefab for the battery item.")]
    [SerializeField] private GameObject batteryPrefab;

    [Tooltip("Prefab for the coin item.")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("Chance for a second drop (percentage between 0 and 100).")]
    [SerializeField] private float secondDropChance = 15f;

    [Header("Flashlight Settings")]
    [Tooltip("Spotlight component for the flashlight.")]
    [SerializeField] private Light flashlightLight; // Reference to the Spot Light

    [Tooltip("Maximum range of the flashlight beam.")]
    [SerializeField] private float maxLightRange = 10f;

    [Tooltip("Maximum intensity of the flashlight beam.")]
    [SerializeField] private float maxLightIntensity = 5f;

    [Tooltip("Duration for which the flashlight effect is visible.")]
    [SerializeField] private float beamDuration = 0.5f;

    [Tooltip("Layer for detecting ghosts.")]
    [SerializeField] private LayerMask ghostLayer;

    [Tooltip("Range of the flashlight for detecting ghosts.")]
    [SerializeField] private float detectionRange = 5f;

    [Tooltip("Width of the detection area.")]
    [SerializeField] private float detectionWidth = 2f;

    [Tooltip("Height of the detection area.")]
    [SerializeField] private float detectionHeight = 2f;

    [Header("Input Action")]
    [Tooltip("Input action for toggling the flashlight.")]
    [SerializeField] private InputAction flashlightAction;

    private float beamTimer = 0f;
    private bool isBeamActive = false;

    private float defaultLightRange;
    private float defaultLightIntensity;

    private void OnEnable()
    {
        flashlightAction.Enable();
    }

    private void OnDisable()
    {
        flashlightAction.Disable();
    }

    private void OnValidate()
    {
        if (flashlightAction == null || flashlightAction.bindings.Count == 0)
        {
            flashlightAction = new InputAction(type: InputActionType.Button);
            flashlightAction.AddBinding("<Keyboard>/f");
        }
    }

    private void Start()
    {
        if (flashlightLight != null)
        {
            defaultLightRange = flashlightLight.range;
            defaultLightIntensity = flashlightLight.intensity;
        }
    }

    private void Update()
    {
        // Check if the flashlight action was triggered
        if (flashlightAction.triggered)
        {
            UseFlashlight();
        }

        // Handle beam effect duration
        if (isBeamActive)
        {
            beamTimer -= Time.deltaTime;
            if (beamTimer <= 0f)
            {
                ResetFlashlightLight();
            }
        }
    }

    private void UseFlashlight()
    {
        if (GameManager.Instance.GetCurrentBatteries() <= 0)
        {
            Debug.LogWarning("Cannot use flashlight. No batteries.");
            return;
        }

        ActivateFlashlightLight();

        GameManager.Instance.UseBattery();

        KillGhosts();
    }

    private void ActivateFlashlightLight()
    {
        if (flashlightLight == null)
        {
            Debug.LogError("No flashlight light assigned!");
            return;
        }

        flashlightLight.range = maxLightRange;
        flashlightLight.intensity = maxLightIntensity;

        isBeamActive = true;
        beamTimer = beamDuration;

        Debug.Log("Flashlight activated visually.");
    }

    private void ResetFlashlightLight()
    {
        if (flashlightLight == null) return;

        flashlightLight.range = defaultLightRange;
        flashlightLight.intensity = defaultLightIntensity;

        isBeamActive = false;

        Debug.Log("Flashlight reset to default.");
    }

    private void KillGhosts()
    {
        Debug.Log("Checking for ghosts in range...");

        // Define the center of the rectangle in front of the player
        Vector3 boxCenter = transform.position + transform.forward * (detectionRange / 2);

        // Define the size of the rectangle (width, height, and length)
        Vector3 boxSize = new Vector3(detectionWidth, detectionHeight, detectionRange);

        // Detect all colliders within the rectangle
        Collider[] hits = Physics.OverlapBox(boxCenter, boxSize / 2, transform.rotation, ghostLayer);

        if (hits.Length == 0)
        {
            Debug.Log("No ghosts detected.");
            return;
        }

        foreach (var hit in hits)
        {
            GhostController ghost = hit.GetComponent<GhostController>();
            if (ghost != null)
            {
                ghost.TakeDamage(); // Kill or damage the ghost
                Debug.Log($"Ghost hit and damaged: {ghost.name}");

                // Handle item drops
                DropItems(hit.transform.position);
            }
            else
            {
                Debug.LogWarning($"Detected object is not a ghost: {hit.name}");
            }
        }
    }

    private void DropItems(Vector3 position)
    {
        // List of possible items to drop
        List<GameObject> itemPrefabs = new List<GameObject> { heartPrefab, batteryPrefab, coinPrefab };

        // Ensure we have all the necessary prefabs assigned
        if (itemPrefabs.Exists(prefab => prefab == null))
        {
            Debug.LogWarning("One or more item prefabs are not assigned in the inspector!");
            return;
        }

        // Adjust the initial position for the first drop to appear slightly above the ground
        Vector3 firstDropPosition = position + Vector3.up * 1f; // Drop 0.5 units higher

        // Drop the first random item
        int firstDropIndex = Random.Range(0, itemPrefabs.Count);
        Instantiate(itemPrefabs[2], firstDropPosition, Quaternion.identity);
        Debug.Log($"Dropped {itemPrefabs[firstDropIndex].name} at {firstDropPosition}");

        // 15% chance to drop another random item
        if (Random.Range(0f, 100f) < secondDropChance)
        {
            // Adjust the position for the second drop to be slightly aside and above the first drop
            Vector3 secondDropPosition = position + Vector3.up * 1f + Vector3.right * 0.3f; // Slightly higher and to the side

            int secondDropIndex = Random.Range(0, itemPrefabs.Count);
            Instantiate(itemPrefabs[secondDropIndex], secondDropPosition, Quaternion.identity);
            Debug.Log($"Dropped another {itemPrefabs[secondDropIndex].name} at {secondDropPosition}");
        }
    }

    private void OnDrawGizmosSelected()
    {  
        Gizmos.color = Color.yellow;

        // Define the center of the rectangle in front of the player
        Vector3 boxCenter = transform.position + transform.forward * (detectionRange / 2);

        // Define the size of the rectangle (width, height, and length)
        Vector3 boxSize = new Vector3(detectionWidth, detectionHeight, detectionRange);

        // Draw the rectangle in the Scene view
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
