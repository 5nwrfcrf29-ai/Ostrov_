using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction crouchAction;

    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float xSens = 5f;
    public float ySens = 5f;

    public Transform cameraHolder;
    public Transform groundCheck;

    float xrot = 0f;

    public float jumpSpeed = 5f;
    Rigidbody rb;

    [Header("Crouch Settings")]
    public float crouchScaleY = 0.5f;
    private float defaultScaleY;

    // [x] xRotation
    // [x] clamp rotation
    // [] rotation based movement
    // [] jump :)

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        Cursor.lockState = CursorLockMode.Locked;
        defaultScaleY = transform.localScale.y;
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        // 1. Check Input States
        bool isCrouching = crouchAction.IsPressed();
        bool isSprinting = sprintAction.IsPressed() && !isCrouching; // Can't sprint while crouching

        // 2. Handle Crouch Scale
        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, defaultScaleY, transform.localScale.z);
        }

        // 3. Determine Speed
        float currentSpeed = speed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        // 4. Movement Logic
        var move = moveAction.ReadValue<Vector2>();
        var movement = new Vector3(move.x, 0, move.y);
        var dir = transform.rotation * movement;
        dir.Normalize();
        dir *= currentSpeed;
        dir.y = rb.linearVelocity.y;


        if (jumpAction.triggered && isGrounded)
        {
            dir.y = jumpSpeed;
        }
        rb.linearVelocity = dir;



        var look = lookAction.ReadValue<Vector2>();
        var xLook = look.x * xSens;
        var yLook = -look.y * ySens;

        xrot += yLook * Time.deltaTime;

        xrot = Mathf.Clamp(xrot, -80, 80);

        cameraHolder.localRotation = Quaternion.Euler(xrot, 0, 0);
        transform.Rotate(0, xLook * Time.deltaTime, 0);


    }

    // je to default :)
    private bool IsGrounded()
    {
        return Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            0.04f
            );
    }
}