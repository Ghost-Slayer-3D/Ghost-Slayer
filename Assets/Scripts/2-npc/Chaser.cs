using System;
using UnityEngine;
using UnityEngine.AI;

/**
 * This component represents an enemy NPC that chases the player.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class Chaser : MonoBehaviour
{
    [Tooltip("The transform of the player to chase (e.g., PlayerBody)")]
    [SerializeField] private Transform targetTransform = null;

    [Header("These fields are for display only")]
    [SerializeField] private Vector3 targetPosition;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Automatically find the target transform
        if (targetTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Assign the transform of the player
                targetTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure your player object is tagged as 'Player'.");
            }
        }
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Target Transform is not assigned or found!");
            return;
        }

        // Update target position
        targetPosition = targetTransform.position;

        // Chase the player
        FacePlayer();
        navMeshAgent.destination = targetPosition;
    }

    private void FacePlayer()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * 4
        );
    }

    internal Vector3 TargetObjectPosition()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Target Transform is not assigned!");
            return transform.position; // Default to current position if target is missing
        }

        return targetTransform.position;
    }
}
