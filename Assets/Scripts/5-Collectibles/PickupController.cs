using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum PickupType
    {
        Heart,
        Battery,
        Coin
    }

    [Header("Pickup Settings")]
    [SerializeField] private PickupType pickupType; // Type of pickup (Heart, Battery, Coin)
    [SerializeField] private int value = 1;         // Amount to add (1 HP, 1 Battery, 10 Coins)

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            if (CanPickup()) // Check if pickup is allowed
            {
                HandlePickup();
                Destroy(gameObject); // Destroy the pickup
            }
        }
    }

    // Checks whether the player can pick up this item
    private bool CanPickup()
    {
        switch (pickupType)
        {
            case PickupType.Heart:
                // Check if health is already full
                return GameManager.Instance.GetCurrentHearts() < GameManager.Instance.GetMaxHearts();

            case PickupType.Battery:
                // Check if battery is already full
                return GameManager.Instance.GetCurrentBatteries() < GameManager.Instance.GetMaxBatteries();

            case PickupType.Coin:
                // Coins can always be picked up
                return true;
        }

        return false; // Default (should never happen)
    }

    // Handles the pickup logic based on type
    private void HandlePickup()
    {
        switch (pickupType)
        {
            case PickupType.Heart:
                GameManager.Instance.Heal(value);
                break;

            case PickupType.Battery:
                GameManager.Instance.AddBattery(value);
                break;

            case PickupType.Coin:
                ScoreManager.Instance.AddCoins(value);
                break;
        }
    }
}
