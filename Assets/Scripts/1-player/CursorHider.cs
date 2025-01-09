using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This component lets the player show/hide the cursor and handles cursor visibility during menu transitions.
 */
public class CursorHider : MonoBehaviour
{
    [SerializeField] InputAction toggleCursorAction;

    private bool isPaused = false; // Tracks whether the game is paused

    void OnEnable() { toggleCursorAction.Enable(); }
    void OnDisable() { toggleCursorAction.Disable(); }
    void OnValidate()
    {
        // Provide default bindings for the input actions.
        if (toggleCursorAction == null)
            toggleCursorAction = new InputAction(type: InputActionType.Button);
        if (toggleCursorAction.bindings.Count == 0)
            toggleCursorAction.AddBinding("<Mouse>/rightButton");
    }

    void Start()
    {
        SetCursorState(false); // Hide the cursor initially
    }

    void Update()
    {
        // Toggle cursor visibility with the toggleCursorAction (right-click by default)
        if (toggleCursorAction.WasPerformedThisFrame() && !isPaused)
        {
            bool cursorVisible = !Cursor.visible;
            SetCursorState(cursorVisible);
        }
    }

    public void OnGamePaused()
    {
        isPaused = true;
        SetCursorState(true); // Show the cursor when the game is paused
    }

    public void OnGameResumed()
    {
        isPaused = false;
        SetCursorState(false); // Hide the cursor when the game is resumed
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
