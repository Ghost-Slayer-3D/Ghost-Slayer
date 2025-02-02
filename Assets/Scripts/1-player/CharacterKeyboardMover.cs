using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Controls player movement, including walking, running, crouching, and jumping,
 * with support for shop-purchased upgrades.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover : MonoBehaviour
{
    [Header("Base Movement Settings")]
    [Tooltip("Base speed of player movement, in meters/second.")]
    [SerializeField] private float baseSpeed = 3.5f;

    [Tooltip("Base gravity affecting the player.")]
    [SerializeField] private float baseGravity = 9.81f;

    [Tooltip("Base height the player can jump.")]
    [SerializeField] private float baseJumpHeight = 3.0f;

    [Tooltip("Multiplier for running speed.")]
    [SerializeField] private float runMultiplier = 2.0f;

    [Tooltip("Speed multiplier while crouching.")]
    [SerializeField] private float crouchSpeedMultiplier = 0.2f;

    [Header("Animation Speed Settings")]
    [Tooltip("Base animation speed multiplier (e.g., 1.0 for normal speed, 2.0 for double speed).")]
    [SerializeField] private float animationSpeedMultiplier = 1.5f;

    // Shop-purchased upgrades
    private float speedMultiplier = 1.0f;
    private float jumpMultiplier = 1.0f;
    private bool isInvisible = false;
    private bool unlimitedBattery = false;

    // Player states
    private bool isJumping = false;
    private bool isCrouching = false;

    // Character and input references
    private CharacterController cc;
    private Camera playerCamera;
    private Animator animator;

    [Header("Input Actions")]
    [Tooltip("Input action for movement.")]
    [SerializeField] private InputAction moveAction;

    [Tooltip("Input action for jumping.")]
    [SerializeField] private InputAction jumpAction;

    [Tooltip("Input action for running.")]
    [SerializeField] private InputAction runAction;

    [Tooltip("Input action for crouching.")]
    [SerializeField] private InputAction crouchAction;

    private Vector3 velocity = Vector3.zero;

    // Enable input actions
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        runAction.Enable();
        crouchAction.Enable();
    }

    // Disable input actions
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        runAction.Disable();
        crouchAction.Disable();
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 inputMovement = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputMovement.x, 0, inputMovement.y).normalized;
        move = transform.TransformDirection(move);

        float adjustedSpeed = baseSpeed * speedMultiplier;

        // Handle running
        if (runAction.ReadValue<float>() > 0 && !isCrouching)
        {
            adjustedSpeed *= runMultiplier;
        }

        // Handle crouching
        if (crouchAction.ReadValue<float>() > 0 && IsGrounded())
        {
            if (!isCrouching)
            {
                isCrouching = true;
            }
            adjustedSpeed *= crouchSpeedMultiplier;
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
        }

        // Grounded movement
        if (IsGrounded())
        {
            isJumping = false;
            velocity.y = 0;

            velocity.x = move.x * adjustedSpeed;
            velocity.z = move.z * adjustedSpeed;

            // Handle jumping
            if (jumpAction.triggered)
            {
                velocity.y = Mathf.Sqrt(2 * baseJumpHeight * jumpMultiplier * baseGravity);
                isJumping = true;
            }
        }
        else
        {
            // Airborne movement
            velocity.x = move.x * adjustedSpeed * 0.6f;
            velocity.z = move.z * adjustedSpeed * 0.6f;

            // Apply gravity
            velocity.y -= baseGravity * Time.deltaTime;
        }

        // Apply movement and gravity
        cc.Move(velocity * Time.deltaTime);

        // Update Animator parameters
        UpdateAnimatorParameters();

        // Adjust animation speed
        animator.speed = animationSpeedMultiplier;

        // Handle invisibility visual effect
        UpdateInvisibilityEffect();
    }

    private void UpdateAnimatorParameters()
    {
        if (animator == null) return;

        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        animator.SetFloat("speed", horizontalSpeed);
        animator.SetFloat("verticalVelocity", velocity.y);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isJumping", isJumping);
    }

    private void UpdateInvisibilityEffect()
    {
        if (isInvisible)
        {
            // Add visual effect logic here (e.g., material transparency)
        }
    }

    // Methods to apply shop upgrades
    public void UpgradeSpeed(float multiplier)
    {
        speedMultiplier += multiplier;
    }

    public void UpgradeJump(float multiplier)
    {
        jumpMultiplier += multiplier;
    }

    public void EnableInvisibility(float duration = 120f)
    {
        if (isInvisible)
        {
            Debug.Log("Invisibility is already active!");
            return;
        }

        isInvisible = true;
        Debug.Log($"Player is now invisible for {duration} seconds!");

        // Notify GameManager about invisibility state
        GameManager.Instance.SetPlayerInvisible(true);

        // Start coroutine to disable invisibility after the duration
        StartCoroutine(DisableInvisibilityAfterTime(duration));
    }

    private IEnumerator DisableInvisibilityAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        isInvisible = false;
        Debug.Log("Invisibility has ended!");

        // Notify GameManager about invisibility state
        GameManager.Instance.SetPlayerInvisible(false);
    }

    public void EnableUnlimitedBattery()
    {
        unlimitedBattery = true;
        // Trigger unlimited battery logic
    }

    private bool IsGrounded()
    {
        float groundCheckDistance = 0.2f;
        Vector3 origin = transform.position + Vector3.up * 0.1f; // Start slightly above ground

        // Multi-ray ground check
        bool centerCheck = Physics.Raycast(origin, Vector3.down, groundCheckDistance);
        bool frontCheck = Physics.Raycast(origin + transform.forward * 0.3f, Vector3.down, groundCheckDistance);
        bool backCheck = Physics.Raycast(origin - transform.forward * 0.3f, Vector3.down, groundCheckDistance);
        bool leftCheck = Physics.Raycast(origin - transform.right * 0.3f, Vector3.down, groundCheckDistance);
        bool rightCheck = Physics.Raycast(origin + transform.right * 0.3f, Vector3.down, groundCheckDistance);

        return centerCheck || frontCheck || backCheck || leftCheck || rightCheck;
    }
}
