using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private int health = 1; // Ghost health
    [SerializeField] private int damage = 1; // Damage dealt to player on collision

    private bool isPlayerInvisible = false;
    private bool isDead = false; // Tracks if the ghost is already dead

    private DamageIndicator damageIndicator; // Reference to the DamageIndicator script

    public delegate void GhostDeathHandler(GameObject ghost);
    public event GhostDeathHandler OnGhostDeath;

    private void Start()
    {
        // Cache the DamageIndicator component
        damageIndicator = GetComponent<DamageIndicator>();
        if (damageIndicator == null)
        {
            Debug.LogError("DamageIndicator component is missing on " + gameObject.name);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return; // Prevent duplicate hits

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // Show floating text via DamageIndicator
        if (damageIndicator != null)
        {
            damageIndicator.ShowDamage("Ghost Defeated!", Color.white); // Display the text when the ghost dies
        }
        // Notify the spawner about the ghost's death
        OnGhostDeath?.Invoke(gameObject);

        // Destroy the ghost object
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return; // Prevent actions if already dead

        if (isPlayerInvisible)
        {
            Die(); // Ghost disappears if the player is invisible
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.IsUnlimitedHP())
                {
                    Die(); // Ignore damage if Unlimited HP is active
                    return;
                }

                // Deal damage to the player
                GameManager.Instance.TakeDamage(damage);

                // Show damage indicator above the player
                ShowDamageIndicator(collision.gameObject.transform);
            }

            Die(); // Destroy the ghost after attacking
        }
    }

    private void ShowDamageIndicator(Transform playerTransform)
    {
        DamageIndicator playerDamageIndicator = playerTransform.GetComponent<DamageIndicator>();
        if (playerDamageIndicator != null)
        {
            playerDamageIndicator.ShowDamage("-1", Color.white);
        }
        else
        {
            Debug.LogWarning("DamageIndicator script missing on player.");
        }
    }

    public void OnPlayerInvisibilityChanged(bool state)
    {
        isPlayerInvisible = state; // Update invisibility state

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = !isPlayerInvisible; // Enable/disable collision
        }
        else
        {
            Debug.LogWarning("Collider component is missing on " + gameObject.name);
        }
    }
}
