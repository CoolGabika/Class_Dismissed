using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Animator animator;

    private PlayerInputActions inputActions;
    private Vector2 input;

    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => input = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => input = Vector2.zero;

        if (animator != null)
            animator.applyRootMotion = false;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        // Gravity
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 move = moveDir * moveSpeed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // Rotation
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            if (animator != null)
                animator.SetBool("Walk", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Walk", false);
        }
    }
}