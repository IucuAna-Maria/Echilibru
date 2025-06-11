using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerActionsInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
{
    public PlayerControls PlayerControls { get; private set; }
    public bool AttackPressed { get; private set; }

    public bool InputEnabled { get; private set; } = true;

    public void EnableInput() => InputEnabled = true;
    public void DisableInput()
    {
        InputEnabled = false;
        AttackPressed = false;
    }

    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerActionMap.Enable();
        PlayerControls.PlayerActionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerActionMap.Disable();
        PlayerControls.PlayerActionMap.RemoveCallbacks(this);
    }

    public void SetAttackingPressedFalse()
    {
        AttackPressed = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!InputEnabled || !context.performed)
            return;

        AttackPressed = true;
    }
}
