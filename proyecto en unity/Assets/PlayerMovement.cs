using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset inputAction;
    public CharacterController playerCharacterController;
    [SerializeField] private Transform playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float playerWalkSpeed = 10f;
    [SerializeField] private float playerRotateDampening = 0.1f;

    [Header("Physics & Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -19.62f;

    private InputAction playerMove;
    private InputAction playerJump;

    private float turnSmoothingVelocity;
    private float verticalVelocity;
    private Vector2 playerMoveAmount;

    private void Awake()
    {
        var playerMap = inputAction.FindActionMap("Player");
        playerMove = playerMap.FindAction("Move");
        playerJump = playerMap.FindAction("Jump");
    }

    private void OnEnable() => inputAction.FindActionMap("Player").Enable();
    private void OnDisable() => inputAction.FindActionMap("Player").Disable();

    private void Update()
    {
        playerMoveAmount = playerMove.ReadValue<Vector2>();

        ApplyJumpAndGravity();
        PlayerMoveAndRotate();
    }

    private void PlayerMoveAndRotate()
    {
        Vector3 direction = new Vector3(playerMoveAmount.x, 0f, playerMoveAmount.y).normalized;
        Vector3 moveDirection = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, playerRotateDampening);

            transform.rotation = Quaternion.Euler(0f, smoothTargetAngle, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        Vector3 velocity = (moveDirection * playerWalkSpeed) + (Vector3.up * verticalVelocity);
        playerCharacterController.Move(velocity * Time.deltaTime);
    }

    private void ApplyJumpAndGravity()
    {
        bool isGrounded = playerCharacterController.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            // Small grounding force to keep the player attached to ground/slopes
            verticalVelocity = -2f;
        }

        if (isGrounded && playerJump.WasPressedThisFrame())
        {
            // Kinematic equation to achieve exact jump height in Unity units
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity continuously every frame
        verticalVelocity += gravity * Time.deltaTime;
    }
}