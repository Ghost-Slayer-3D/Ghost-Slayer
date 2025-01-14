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
    [SerializeField] private Light flashlightLight;

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

    [Header("Animator")]
    [Tooltip("Animator component for the character.")]
    [SerializeField] private Animator animator;

    private float beamTimer = 0.5f;
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
        if (flashlightAction.triggered)
        {
            TriggerFlashlight();
        }

        if (isBeamActive)
        {
            beamTimer -= Time.deltaTime;
            if (beamTimer <= 0f)
            {
                ResetFlashlightLight();
            }
        }
    }

    private void TriggerFlashlight()
    {
        if (!GameManager.Instance.IsUnlimitedBattery() && GameManager.Instance.GetCurrentBatteries() <= 0)
        {
            Debug.Log("No batteries left to use the flashlight!");
            return;
        }

        ActivateFlashlightLight();

        if (!GameManager.Instance.IsUnlimitedBattery())
        {
            GameManager.Instance.UseBattery();
        }

        KillGhosts();

        if (animator != null)
        {
            animator.SetTrigger("AttackTrigger");
        }
    }

    private void ActivateFlashlightLight()
    {
        if (flashlightLight == null)
        {
            return;
        }

        flashlightLight.range = maxLightRange;
        flashlightLight.intensity = maxLightIntensity;

        isBeamActive = true;
        beamTimer = beamDuration;
    }

    private void ResetFlashlightLight()
    {
        if (flashlightLight == null)
        {
            return;
        }

        flashlightLight.range = defaultLightRange;
        flashlightLight.intensity = defaultLightIntensity;

        isBeamActive = false;
    }

    private void KillGhosts()
    {
        Vector3 boxCenter = transform.position + transform.forward * (detectionRange / 2);
        Vector3 boxSize = new Vector3(detectionWidth, detectionHeight, detectionRange);

        Collider[] hits = Physics.OverlapBox(boxCenter, boxSize / 2, transform.rotation, ghostLayer);

        foreach (var hit in hits)
        {
            GhostController ghost = hit.GetComponent<GhostController>();
            if (ghost != null)
            {
                ghost.TakeDamage();
                DropItems(hit.transform.position);
            }
        }
    }

    private void DropItems(Vector3 position)
    {
        List<GameObject> itemPrefabs = new List<GameObject> { heartPrefab, batteryPrefab, coinPrefab };

        if (itemPrefabs.Exists(prefab => prefab == null))
        {
            return;
        }

        Vector3 firstDropPosition = position + Vector3.up * 1f;

        int firstDropIndex = Random.Range(0, itemPrefabs.Count);
        Instantiate(itemPrefabs[2], firstDropPosition, Quaternion.identity);

        if (Random.Range(0f, 100f) < secondDropChance)
        {
            Vector3 secondDropPosition = position + Vector3.up * 1f + Vector3.right * 0.3f;
            int secondDropIndex = Random.Range(0, itemPrefabs.Count);
            Instantiate(itemPrefabs[secondDropIndex], secondDropPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 boxCenter = transform.position + transform.forward * (detectionRange / 2);
        Vector3 boxSize = new Vector3(detectionWidth, detectionHeight, detectionRange);

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
