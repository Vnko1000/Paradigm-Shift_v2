using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Paradigm Shift/Input Reader")]
public class InputReader:ScriptableObject,PlayerInputActions.IPlayerActions
{
    // Eventos que otros sistemas escuchan (Observer pattern)
    public event System.Action<Vector2> OnMoveEvent;
    public event System.Action OnJumpEvent;
    public event System.Action<bool> OnCrouchEvent;    // true = pressed, false = released
    public event System.Action<bool> OnSprintEvent;    // true = pressed, false = released
    public event System.Action OnSableToggleEvent;
    public event System.Action OnInteractEvent;

    private PlayerInputActions inputActions;

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            inputActions.Player.SetCallbacks(this);
        }
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions?.Player.Disable();
    }

    // Callbacks del Input System (implementación de IPlayerActions)
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJumpEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        OnCrouchEvent?.Invoke(context.ReadValueAsButton());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        OnSprintEvent?.Invoke(context.ReadValueAsButton());
    }

    public void OnSable(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSableToggleEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteractEvent?.Invoke();
    }
}