using UnityEngine;

/// <summary>
/// Controls the behavior of the ghost, including health, damage, and death effects.
/// </summary>
public class GhostController : MonoBehaviour
{
    // -------------------------------
    // Serialized Fields
    // -------------------------------

    [Header("Ghost Settings")]
    [SerializeField] private int health = 1;                 // Ghost health (can be extended later).
    [SerializeField] private int damage = 1;                 // Damage dealt to player on collision.
    [SerializeField] private GameObject deathEffect;         // Visual effect for death.

    // -------------------------------
    // Private Fields
    // -------------------------------

    private bool isDead = false;                             // Tracks if the ghost is already dead.

    // -------------------------------
    // Damage Handling
    // -------------------------------

    /// <summary>
    /// Called when the ghost is hit by the flashlight.
    /// </summary>
    public void TakeDamage()
    {
        if (isDead) return; // Prevent duplicate hits

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Called when the ghost is hit by the flashlight, with a delay if needed.
    /// Different from Die() for flexibility in effects.
    /// </summary>
    public void DestroyGhost()
    {
        Die();
    }

    // -------------------------------
    // Death Handling
    // -------------------------------

    /// <summary>
    /// Handles the death of the ghost, including visual effects and destruction.
    /// </summary>
    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // Play death effect if assigned
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the ghost object
        Destroy(gameObject);
    }

    // -------------------------------
    // Collision Handling
    // -------------------------------

    /// <summary>
    /// Handles collision with the player and deals damage.
    /// </summary>
    /// <param name="collision">Collision object.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            GameManager.Instance.TakeDamage(damage);
            Die();
        }
    }
}
