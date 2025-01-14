using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private int health = 1;                 // Ghost health (can be extended later).
    [SerializeField] private int damage = 1;                 // Damage dealt to player on collision.
    [SerializeField] private GameObject deathEffect;         // Visual effect for death.
    private bool isPlayerInvisible = false;
    private bool isDead = false;                             // Tracks if the ghost is already dead.

    public void TakeDamage()
    {
        if (isDead) return; // Prevent duplicate hits

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    public void DestroyGhost()
    {
        Die();
    }

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

    public void OnPlayerInvisibilityChanged(bool state)
    {
        isPlayerInvisible = state; // Assign the state directly to isPlayerInvisible

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = !isPlayerInvisible; // Enable/disable collision based on invisibility
        }
        else
        {
            Debug.LogWarning("Collider component is missing on " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayerInvisible){
            Die();
            return; // Do nothing if the player is invisible
        } 

        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.IsUnlimitedHP())
                {
                    Die();
                    return; // Ignore damage if Unlimited HP is active
                }

                GameManager.Instance.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("GameManager instance is missing!");
            }

            Die(); // Kill the ghost
        }
    }
}
