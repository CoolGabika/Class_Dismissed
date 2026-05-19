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
        // 1. Získanie smeru a jeho normalizácia (zabraňuje rýchlejšiemu pohybu po uhlopriečke)
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        // 2. Gravitácia (v Unity 6 funguje CharacterController rovnako)
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // Drží postavu pevne na zemi
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 3. Výpočet výsledného pohybu
        Vector3 move = moveDir * moveSpeed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // 4. PREPOJENIE S BLEND TREE V UNITY 6
        if (animator != null)
        {
            // Posielame surové hodnoty z Input Systemu priamo do parametrov Blend Tree
            animator.SetFloat("MoveX", input.x);
            animator.SetFloat("MoveY", input.y);
        }

        // 5. Otáčanie postavy v smere pohybu
        // POZNÁMKA: Ak robíš hru, kde postava môže cúvať čelom ku kamere (napr. strieľačka),
        // toto otáčanie budeš musieť neskôr upraviť/odstrániť. Ak ide o adventúru, nechaj to takto.
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}