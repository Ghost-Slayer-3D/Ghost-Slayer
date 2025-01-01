using UnityEngine;
using UnityEngine.InputSystem;

public class VillagerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 3f; // Range to detect villagers

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
                break;
            }
        }

        if (currentVillager == null)
        {
        }
    }

    private void TryInteractWithVillager(InputAction.CallbackContext context)
    {
        // Interact only if a villager is detected
        if (currentVillager != null)
        {
            currentVillager.Interact();
        }
        else
        {
        }
    }
}
