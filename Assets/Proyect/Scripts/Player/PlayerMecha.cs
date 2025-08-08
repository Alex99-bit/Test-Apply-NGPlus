using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMecha : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;
    
    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    [Header("Camera Reference")]
    public Transform cameraTransform;

    [Header("Animation")]
    public Animator animator; // Asigna tu Animator en el inspector

    private CharacterController controller;
    private float currentSpeed;
    private Vector3 velocity;
    private bool isGrounded;
    private Transform groundCheck;
    private string lastState; // Guarda el último estado activado

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        currentSpeed = walkSpeed;

        // Crear punto de GroundCheck
        groundCheck = new GameObject("GroundCheck").transform;
        groundCheck.parent = transform;
        groundCheck.localPosition = new Vector3(0, -1, 0);
    }

    void Update()
    {
        if (!controller.enabled || !gameObject.activeInHierarchy)
            return;

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Shift = correr
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        currentSpeed = isRunning && verticalInput > 0 ? runSpeed : walkSpeed;

        // Dirección
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Movimiento
        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;
            Vector3 movement = moveDirection * currentSpeed;
            controller.Move(movement * Time.deltaTime);

            // Rotación suave
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Animaciones
        HandleAnimation(inputDirection.magnitude, isRunning);

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleAnimation(float movementMagnitude, bool isRunning)
    {
        string newState;

        if (movementMagnitude < 0.1f)
            newState = "Idle";
        else if (isRunning)
            newState = "Run";
        else
            newState = "Walk";

        if (newState != lastState)
        {
            // Resetea triggers antes de activar el nuevo
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Run");

            animator.SetTrigger(newState);
            lastState = newState;
        }
    }
}
