using UnityEngine;
using UnityEngine.AI;

/**
 * This component represents an enemy NPC that shoots the player when it is nearby.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Tooltip("The object that this enemy chases after")]
    [SerializeField] private GameObject player = null;

    [Tooltip("An effect for creating a flash near the rifle mouth during shooting")]
    [SerializeField] private GameObject muzzleFlash = null;

    [Tooltip("An effect for creating a bullet-hole where the enemy shoots")]
    [SerializeField] private GameObject bulletHole = null;

    [SerializeField] private float lookRadius = 10f;

    [Header("These fields are for display only")]
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private bool isShooting = false;

    private AudioSource audioSource;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool isPlayerInvisible = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzleFlash.SetActive(false);
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPlayerInvisible)
        {
            // Stop any logic related to attacking or chasing the player
            muzzleFlash.SetActive(false);
            isShooting = false;
            return;
        }

        // Existing logic when the player is visible
        playerPosition = player.transform.position;
        float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);

        if (distanceToPlayer <= lookRadius)
        {
            FacePlayer();
            ShootPlayer();
        }
        else
        {
            muzzleFlash.SetActive(false);
            isShooting = false;
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(
            new Vector3(direction.x, 0, direction.z)
        );

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * 5
        );
    }

    private void FaceDirection()
    {
        Vector3 direction = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(
            new Vector3(direction.x, 0, direction.z)
        );

        // Smoothly rotate towards the destination
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * 5
        );
    }

    private void ShootPlayer()
    {
        isShooting = true;
        audioSource.Play();
        muzzleFlash.SetActive(true);

        // Position ray casted from
        Ray rayOrigin = new Ray(
            transform.position + Vector3.up * 1.5f,
            transform.forward + new Vector3(0.2f, 0.2f, 0.2f)
        );

        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            GameObject hitMarker = Instantiate(
                bulletHole,
                hitInfo.point,
                Quaternion.LookRotation(hitInfo.normal)
            ) as GameObject;

            Destroy(hitMarker, 1f);
        }
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public void OnPlayerInvisibilityChanged(bool state)
    {
        isPlayerInvisible = state;

        if (isPlayerInvisible)
        {
            Debug.Log("Player is invisible, stopping enemy tracking.");
            navMeshAgent.isStopped = true; // Stop movement
            muzzleFlash.SetActive(false); // Stop shooting
            isShooting = false;
        }
        else
        {
            Debug.Log("Player is visible, resuming enemy tracking.");
            navMeshAgent.isStopped = false; // Resume movement
        }
    }
}
