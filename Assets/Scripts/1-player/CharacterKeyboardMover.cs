using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Controls player movement, including walking, running, crouching, and jumping.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of player keyboard-movement, in meters/second")]
    [SerializeField] private float speed = 3.5f;

    [Tooltip("Gravity affecting the player.")]
    [SerializeField] private float gravity = 9.81f;

    [Tooltip("Height the player can jump.")]
    [SerializeField] private float jumpHeight = 3.0f;

    [Tooltip("Multiplier for running speed.")]
    [SerializeField] private float runMultiplier = 2.0f;

    [Tooltip("Speed multiplier while crouching.")]
    [SerializeField] private float crouchSpeedMultiplier = 0.2f;

    // Player states
    private bool isJumping = false;
    private bool isCrouching = false;

    // Character and input references
    private CharacterController cc;
    private Camera playerCamera;
    private Animator animator; // Animator reference

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

    // Validate bindings in editor
    private void OnValidate()
    {
        // Movement bindings
        if (moveAction == null || moveAction.bindings.Count == 0)
        {
            moveAction = new InputAction(type: InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
        }

        // Jump bindings
        if (jumpAction == null || jumpAction.bindings.Count == 0)
        {
            jumpAction = new InputAction(type: InputActionType.Button);
            jumpAction.AddBinding("<Keyboard>/space");
        }

        // Run bindings
        if (runAction == null || runAction.bindings.Count == 0)
        {
            runAction = new InputAction(type: InputActionType.Button);
            runAction.AddBinding("<Keyboard>/leftShift");
        }

        // Crouch bindings
        if (crouchAction == null || crouchAction.bindings.Count == 0)
        {
            crouchAction = new InputAction(type: InputActionType.Button);
            crouchAction.AddBinding("<Keyboard>/leftCtrl");
        }
    }

    // Initialize components
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        animator = GetComponent<Animator>(); // Reference the Animator
    }

    // Handle player movement
    private void Update()
    {
        Vector2 inputMovement = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputMovement.x, 0, inputMovement.y).normalized;
        move = transform.TransformDirection(move);

        float adjustedSpeed = speed;

        // Handle running
        if (runAction.ReadValue<float>() > 0 && !isCrouching)
        {
            adjustedSpeed *= runMultiplier;
        }

        // Handle crouching
        if (crouchAction.ReadValue<float>() > 0 && cc.isGrounded)
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
        if (cc.isGrounded)
        {
            isJumping = false;
            velocity.y = 0;

            velocity.x = move.x * adjustedSpeed;
            velocity.z = move.z * adjustedSpeed;

            // Handle jumping
            if (jumpAction.triggered)
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                isJumping = true;
            }
        }
        else
        {
            // **Airborne Movement - Allows moving while in air**
            velocity.x = move.x * adjustedSpeed * 0.6f; // 60% reduced speed in air
            velocity.z = move.z * adjustedSpeed * 0.6f;

            // Apply gravity
            velocity.y -= gravity * Time.deltaTime;
        }

        // Apply movement and gravity
        cc.Move(velocity * Time.deltaTime);

        // Update Animator parameters
        UpdateAnimatorParameters();
    }

    /**
     * Updates Animator parameters to match player state.
     */
    private void UpdateAnimatorParameters()
    {
        if (animator == null) return;

        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        animator.SetFloat("speed", horizontalSpeed); // Update speed parameter
        animator.SetFloat("verticalVelocity", velocity.y); // Update vertical velocity parameter
        animator.SetBool("isGrounded", cc.isGrounded); // Update grounded state
        animator.SetBool("isJumping", isJumping); // Update jumping state
    }
}
