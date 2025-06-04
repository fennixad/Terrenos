using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator anim;

    private Vector3 velocity;
    private float rotationVelocity;
    Vector2 inputMov;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        RefreshAnimator();
    }

    void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;

        // Aplicar gravedad
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        // Entrada de movimiento
        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.y = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(inputMov.x, 0f, inputMov.y).normalized;

        // Animación
        anim.SetFloat("movement", inputDir.magnitude);

            float targetAngle = cameraTransform.localEulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 moveDir = cameraTransform.right * inputMov.x + cameraTransform.forward * inputMov.y;
            moveDir.y = 0f;
            moveDir.Normalize();

            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Movimiento vertical (gravedad)
        controller.Move(new Vector3(0f, velocity.y, 0f) * Time.deltaTime);
    }

    void RefreshAnimator()
    {
        bool _seMueve = inputMov.magnitude == 0f ? false : true;
        anim.SetBool("seMueve", _seMueve);
    }

}
