using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/**
 * This component sends its object to a point in the world whenever the player clicks on that point.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class ClickMover : MonoBehaviour
{
    [SerializeField] private bool drawRayForDebug = true;
    [SerializeField] private float rayLength = 100f;
    [SerializeField] private float rayDuration = 1f;
    [SerializeField] private Color rayColor = Color.white;

    [SerializeField] private InputAction moveTo = new InputAction(type: InputActionType.Button);
    [SerializeField] private InputAction moveToLocation = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");

    private NavMeshAgent agent;

    private void OnEnable()
    {
        moveTo.Enable();
        moveToLocation.Enable();
    }

    private void OnDisable()
    {
        moveTo.Disable();
        moveToLocation.Disable();
    }

    private void OnValidate()
    {
        // Provide default bindings for the input actions. Based on https://gamedev.stackexchange.com/a/205345/18261
        if (moveTo.bindings.Count == 0)
        {
            moveTo.AddBinding("<Mouse>/leftButton");
        }

        if (moveToLocation.bindings.Count == 0)
        {
            moveToLocation.AddBinding("<Mouse>/position");
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (moveTo.WasPerformedThisFrame())
        {
            Vector2 mousePositionInScreenCoordinates = moveToLocation.ReadValue<Vector2>();

            // Debug.Log("moving to screen coordinates: " + mousePositionInScreenCoordinates);
            Ray rayFromCameraToClickPosition = Camera.main.ScreenPointToRay(mousePositionInScreenCoordinates);

            if (drawRayForDebug)
            {
                Debug.DrawRay(
                    rayFromCameraToClickPosition.origin,
                    rayFromCameraToClickPosition.direction * rayLength,
                    rayColor,
                    rayDuration
                );
            }

            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(rayFromCameraToClickPosition, out hitInfo);
            if (hasHit)
            {
                // agent.SetDestination(hitInfo.point);
                agent.destination = hitInfo.point;
            }
        }
    }
}
