using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;
    public float inAirAcceleration = 0.15f;
    public float drag = 0.1f;
    public float gravity = 25f;
    public float jumpSpeed = 1.0f;
    public float movingThreshold = 0.01f;

    [Header("Camera Settings")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 89f;

    [Header("Environment Details")]
    [SerializeField] private LayerMask groundLayers;

    private PlayerLocomotionInput playerLocomotionInput;
    private PlayerState playerState;

    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

    private bool jumpedLastFrame = false;
    private float verticalVelocity = 0f;
    private float antiBump;
    private float stepOffset;

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        playerState = GetComponent<PlayerState>();

        antiBump = sprintSpeed;
        stepOffset = characterController.stepOffset;
    }

    private void Update()
    {
        UpdateMovementState();
        HandleVerticalMovement();
        HandleLateralMovement();
    }

    private void UpdateMovementState()
    {
        bool isMovementInput = playerLocomotionInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();
        bool isSprinting = playerLocomotionInput.SprintToggleOn && isMovingLaterally;
        bool isGrounded = IsGrounded();

        PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                           isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
        playerState.SetPlayerMovementState(lateralState);

        if ((!isGrounded || jumpedLastFrame) && characterController.velocity.y > 0f)
        {
            playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
            jumpedLastFrame = false;
            characterController.stepOffset = 0f;
        }
        else if ((!isGrounded || jumpedLastFrame) && characterController.velocity.y <= 0f)
        {
            playerState.SetPlayerMovementState(PlayerMovementState.Falling);
            jumpedLastFrame = false;
            characterController.stepOffset = 0f;
        }
        else
        {
            characterController.stepOffset = stepOffset;
        }
    }

    private void HandleVerticalMovement()
    {
        bool isGrounded = playerState.InGroundedState();
        verticalVelocity -= gravity * Time.deltaTime;

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -antiBump;

        if (playerLocomotionInput.JumpPressed && isGrounded)
        {
            verticalVelocity += antiBump + Mathf.Sqrt(jumpSpeed * 3 * gravity);
            jumpedLastFrame = true;
        }
    }

    private void HandleLateralMovement()
    {
        bool isSprinting = playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isGrounded = playerState.InGroundedState();

        float lateralAcceleration = !isGrounded ? inAirAcceleration :
                                    isSprinting ? sprintAcceleration : runAcceleration;

        float clampLateralMagnitude = !isGrounded ? sprintSpeed :
                                      isSprinting ? sprintSpeed : runSpeed;

        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * playerLocomotionInput.MovementInput.x + cameraForwardXZ * playerLocomotionInput.MovementInput.y;

        Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
        Vector3 newVelocity = characterController.velocity + movementDelta;

        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
        newVelocity.y += verticalVelocity;

        characterController.Move(newVelocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        cameraRotation.x += lookSenseH * playerLocomotionInput.LookInput.x;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y - lookSenseV * playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * playerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
    }

    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.y);

        return lateralVelocity.magnitude > movingThreshold;
    }

    private bool IsGrounded()
    {
        bool grounded = playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();

        return grounded;
    }

    private bool IsGroundedWhileGrounded()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - characterController.radius, transform.position.z);
        bool grounded = Physics.CheckSphere(spherePosition, characterController.radius, groundLayers, QueryTriggerInteraction.Ignore);
        
        return grounded;
    }

    private bool IsGroundedWhileAirborne()
    {
        return characterController.isGrounded;
    }
}