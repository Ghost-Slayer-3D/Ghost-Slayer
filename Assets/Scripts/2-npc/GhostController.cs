using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    [SerializeField] private int health = 1;            // Ghost health (can be extended later).
    [SerializeField] private int damage = 1;           // Damage to player on collision.
    [SerializeField] private GameObject deathEffect;   // Visual effect for death.

    private bool isDead = false;                       // Track if the ghost is already dead.

    // Called when hit by flashlight
    public void TakeDamage()
    {
        if (isDead) return; // Prevent duplicate hits

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    // Called when hit by flashlight
    // different from Die method because it has a delay
    public void DestroyGhost()
    {   
        Die();
    }


    // Trigger destruction and effects
    private void Die()
    {
        isDead = true;

        // Play death effect if assigned
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the ghost
        Destroy(gameObject);
    }

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
