using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] public float interactionRange = 3f; // Range to detect villagers
    private Villager currentVillager;

    void Update()
    {
        DetectVillager();

        // Interact when 'E' is pressed
        if (Input.GetKeyDown(KeyCode.E) && currentVillager != null)
        {
            currentVillager.Interact(); // Trigger interaction
        }
    }

    void DetectVillager()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        currentVillager = null; // Reset current villager

        foreach (Collider hit in hits)
        {
            Villager villager = hit.GetComponent<Villager>();
            if (villager != null) // Villager script found
            {
                currentVillager = villager;
                break; // Stop after finding the first villager
            }
        }
    }
}
