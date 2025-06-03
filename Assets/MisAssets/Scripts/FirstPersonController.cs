using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;
    Animator anim;

    Vector2 inputMov;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();

        RefreshAnimator();
    }

    private void Reset()
    {
        controller = GetComponent<CharacterController>();
        controller.center = Vector3.up;
        controller.radius = 0.3f;
        controller.skinWidth = 0.0001f;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // pequeña fuerza hacia abajo para mantenerse en el suelo
        }

        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.y = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * inputMov.x + transform.forward * inputMov.y;
        move.Normalize();

        controller.Move(move * walkSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void RefreshAnimator()
    {
        bool _seMueve = inputMov.magnitude == 0f ? false : true;
        anim.SetBool("seMueve", _seMueve);
    }


}