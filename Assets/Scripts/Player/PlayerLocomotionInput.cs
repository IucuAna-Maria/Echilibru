using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    [SerializeField] private bool holdToSprint = true;

    public bool SprintToggleOn {  get; private set; }
    public PlayerControls PlayerControls {  get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }

    public bool InputEnabled { get; private set; } = true;

    public void EnableInput() => InputEnabled = true;
    public void DisableInput()
    {
        InputEnabled = false;
        MovementInput = Vector2.zero;
        LookInput = Vector2.zero;
        JumpPressed = false;
        SprintToggleOn = false;
    }



    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        JumpPressed = false;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!InputEnabled) return;

        MovementInput = context.ReadValue<Vector2>();
        //print(MovementInput);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!InputEnabled) return;

        LookInput = context.ReadValue<Vector2>();
    }

    public void OnToogleSprint(InputAction.CallbackContext context)
    {
        if (!InputEnabled) return;

        if (context.performed)
        {
            SprintToggleOn = holdToSprint || !SprintToggleOn;
        }
        else if (context.canceled)
        {
            SprintToggleOn = !holdToSprint && SprintToggleOn;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!InputEnabled || !context.performed)
            return;

        JumpPressed = true;
    }
}
