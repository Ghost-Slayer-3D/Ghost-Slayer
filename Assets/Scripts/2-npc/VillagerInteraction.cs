using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class VillagerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f; // Range to detect villagers
    [SerializeField] private TextMeshProUGUI interactionText; // Reference to TMP text in UI

    private InputAction interactAction; // Input system action
    private Villager currentVillager;

    private void OnEnable()
    {
        // Initialize the 'E' key input action
        interactAction = new InputAction("Interact", binding: "<Keyboard>/e");
        interactAction.Enable();
        interactAction.performed += TryInteractWithVillager;
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void Update()
    {
        DetectVillager();
    }

    private void DetectVillager()
    {
        // Check for colliders within interaction range
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);
        currentVillager = null;

        foreach (Collider hit in hits)
        {
            // Look for a Villager component
            Villager villager = hit.GetComponent<Villager>();
            if (villager != null)
            {
                currentVillager = villager; // Assign the villager if found
                Debug.Log($"Detected villager: {villager.name}"); // Debug log for detection
                break;
            }
        }

        // Show or hide interaction text based on villager presence
        if (currentVillager != null)
        {
            if (currentVillager is ShopVillager)
            {
                interactionText.text = "Press E to open the shop";
            }
            else
            {
                interactionText.text = "Press E to interact";
            }

            interactionText.gameObject.SetActive(true);
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void TryInteractWithVillager(InputAction.CallbackContext context)
    {
        // Interact only if a villager is detected
        if (currentVillager != null)
        {
            Debug.Log($"Interacting with: {currentVillager.name}"); // Debug log for interaction
            currentVillager.Interact();
        }
        else
        {
            Debug.LogWarning("No villager detected to interact with."); // Debug log for no interaction
        }
    }
}
