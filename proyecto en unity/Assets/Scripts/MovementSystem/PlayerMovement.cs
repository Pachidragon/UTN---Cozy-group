using UnityEngine;
using UnityEngine.InputSystem;

// Controla el movimiento, rotación, gravedad y salto del jugador.
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;
    [SerializeField] private float speed;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;


    [Header("Gravity")]
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier;
    private float velocity;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    [Header("Camera")]
    private Camera mainCamera;

    private void Awake() // Obtiene las referencias necesarias al iniciar el jugador.
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update() // Actualiza la rotación, gravedad y movimiento del jugador.
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
        
    }

    private void ApplyGravity() // Aplica la gravedad y actualiza la velocidad vertical del jugador.
    {
        if(IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        direction.y = velocity;
    }
    private void ApplyRotation() // Calcula y aplica la rotación del jugador según la dirección de movimiento y la cámara.
    {
        if (input.sqrMagnitude == 0)
        {
            direction = Vector3.zero;
            return;
        }

        direction = Quaternion.Euler(0.0f, mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(input.x, 0.0f, input.y);
        var targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement() // Mueve al jugador utilizando el CharacterController.
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context) // Recibe la entrada de movimiento desde el Input System
    {
        input = context.ReadValue<Vector2>();
        Debug.Log("Player se mueve :)");
    }

    public void Jump(InputAction.CallbackContext context) // Ejecuta el salto cuando el jugador presiona el botón correspondiente.
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        velocity += jumpForce;
    }

    private bool IsGrounded() => characterController.isGrounded; // Comprueba si el jugador está en contacto con el suelo.
}