using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpheight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldRotateTowardsMovement = true;

    private CharacterController controls;
    private Vector3 moveInput;
    private Vector3 velocity;
    private bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = GetComponent<CharacterController>();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move input: {moveInput}");
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping {context.performed} - Is Grounded: {controls.isGrounded}");
        if (context.performed && controls.isGrounded)
        {
            Debug.Log("Jump executed");
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Camera direction
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        InputAction orbitAction = GetComponentInChildren<InputHandler>().horizontal.action;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        float usedSpeed = isSprinting ? sprintSpeed : speed;
        controls.Move(moveDirection * usedSpeed * Time.deltaTime);

        if (shouldRotateTowardsMovement && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        // Jump and Gravity
        velocity.y += gravity * Time.deltaTime;
        controls.Move(velocity * Time.deltaTime);
    }
}
