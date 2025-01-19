using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject ghostPrefab; // Prefab of the ghost to spawn
    [SerializeField] private int maxGhosts = 5; // Maximum number of ghosts
    [SerializeField] private float spawnRadius = 5f; // Radius for spawning
    [SerializeField] private float respawnDelay = 2f; // Delay before respawning a ghost

    private List<GameObject> activeGhosts = new List<GameObject>(); // List of currently spawned ghosts

    private void Start()
    {
        // Spawn initial ghosts
        for (int i = 0; i < maxGhosts; i++)
        {
            SpawnGhost();
        }
    }

    private void SpawnGhost()
    {
        if (ghostPrefab == null)
        {
            Debug.LogError("Ghost Prefab is not assigned!");
            return;
        }

        // Generate a random position within the spawn radius
        Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
        spawnPosition.y = transform.position.y; // Keep the Y position consistent with the spawner

        // Spawn the ghost and add it to the list
        GameObject ghost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        activeGhosts.Add(ghost);

        // Subscribe to the ghost's death event
        GhostController ghostController = ghost.GetComponent<GhostController>();
        if (ghostController != null)
        {
            ghostController.OnGhostDeath += HandleGhostDeath;
        }
    }

    private void HandleGhostDeath(GameObject ghost)
    {
        // Remove the ghost from the active list
        if (activeGhosts.Contains(ghost))
        {
            activeGhosts.Remove(ghost);
        }

        // Respawn after a delay
        StartCoroutine(RespawnGhostAfterDelay());
    }

    private System.Collections.IEnumerator RespawnGhostAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Ensure we only respawn if we're below the max count
        if (activeGhosts.Count < maxGhosts)
        {
            SpawnGhost();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn radius in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
