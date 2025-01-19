using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patroller : MonoBehaviour
{
    [Tooltip("Radius around the ghost within which it patrols randomly.")]
    [SerializeField] private float patrolRadius = 10f;

    [Tooltip("Minimum time to wait at target before choosing a new destination.")]
    [SerializeField] private float minWaitAtTarget = 3f;

    [Tooltip("Maximum time to wait at target before choosing a new destination.")]
    [SerializeField] private float maxWaitAtTarget = 7f;

    [Header("For debugging")]
    [SerializeField] private float timeToWaitAtTarget = 0;

    private NavMeshAgent navMeshAgent;
    private Animator animator; // Optional, for animation
    private float rotationSpeed = 5f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Choose the first destination
        SelectNewRandomDestination();
    }

    private void Update()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            // We are at the target
            if (timeToWaitAtTarget <= 0)
            {
                SelectNewRandomDestination();
            }
            else
            {
                timeToWaitAtTarget -= Time.deltaTime;
            }
        }
        else
        {
            // Rotate towards the destination for a natural look
            FaceDestination();
        }
    }

    private void SelectNewRandomDestination()
    {
        // Generate a random point within the patrol radius
        Vector3 randomPoint = GetRandomPointWithinRadius();

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(randomPoint);

        // Set the wait time for when the target is reached
        timeToWaitAtTarget = Random.Range(minWaitAtTarget, maxWaitAtTarget);

        // Debug: Visualize the chosen destination
        Debug.Log($"New random destination: {randomPoint}");
    }

    private Vector3 GetRandomPointWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position; // Offset by the current position
        randomDirection.y = transform.position.y; // Keep the Y coordinate consistent

        // Check if the point is on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // Fallback: Use the current position if no valid point is found
        return transform.position;
    }

    private void FaceDestination()
    {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(
            new Vector3(directionToDestination.x, 0, directionToDestination.z)
        );

        // Gradual rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the patrol radius in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
