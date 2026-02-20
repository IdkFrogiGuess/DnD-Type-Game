using UnityEngine;
using UnityEngine.InputSystem;


public class FirstPerMovment : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldRotateTowardsMovement = true;

    // Camera control settings
    [SerializeField] private float lookSensitivity = 200f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private bool lockCursor = true;

    private CharacterController controls;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isSprinting;

    // Look input state
    private Vector2 lookInput;
    private float cameraPitch = 0f;

    void Start()
    {
        controls = GetComponent<CharacterController>();
        if (controls == null)
        {
            Debug.LogError("CharacterController component missing on player.");
            enabled = false;
            return;
        }

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerCamera != null)
        {
            cameraPitch = playerCamera.localEulerAngles.x;
            if (cameraPitch > 180f) cameraPitch -= 360f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (enabled == false)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (enabled == false)
            return;

        if (context.performed) isSprinting = true;
        else if (context.canceled) isSprinting = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (enabled == false)
            return;

        if (controls == null) return;
        if (context.performed && controls.isGrounded)
        {
            // Calculate initial jump velocity from height and gravity (gravity should be negative)
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
    }

    // Look action (Vector2)
    public void OnLook(InputAction.CallbackContext context)
    {
        if (enabled == false)
            return;

        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (controls == null) return;

        Vector3 moveDirection = playerCamera.forward * moveInput.y + playerCamera.right * moveInput.x;
        moveDirection.y = 0f;

        float usedSpeed = isSprinting ? sprintSpeed : speed;
        Vector3 horizontalVelocity = moveDirection * usedSpeed;

        // GROUND CHECK & GRAVITY
        if (controls.isGrounded && velocity.y < 0f)
        {
            // Small downward force to keep the controller grounded
            velocity.y = -2f;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal and vertical motion and move once
        Vector3 finalMotion = (horizontalVelocity + Vector3.up * velocity.y) * Time.deltaTime;
        controls.Move(finalMotion);
    }
}
