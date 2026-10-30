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

    [Header("Animations")]
    private Animator animator; // Guarda la referencia a las animaciones de la brujita
    private bool isCrouching = false; // Controla si está agachada o no

    private void Awake() // Obtiene las referencias necesarias al iniciar el jugador.
    {
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;

      

        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        // Busca el componente Animator en el objeto hijo de forma automática
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() // Actualiza la rotación, gravedad y movimiento del jugador.
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();

        //Actualiza los estados del Animator en cada frame
        UpdateAnimatorParameters();
    }

    private void ApplyGravity() // Aplica la gravedad y actualiza la velocidad vertical del jugador.
    {
        if (IsGrounded() && velocity < 0.0f)
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
        // Si está agachada, reduce la velocidad a la mitad para que no corra agachada
        float currentSpeed = isCrouching ? (speed * 0.5f) : speed;

        // Modificamos el movimiento horizontal manteniendo la velocidad de caída vertical limpia
        Vector3 finalMovement = new Vector3(direction.x * currentSpeed, direction.y, direction.z * currentSpeed);
        characterController.Move(finalMovement * Time.deltaTime);
    }

    private void UpdateAnimatorParameters() // Envía los parámetros de movimiento y suelo al Animator
    {
        if (animator == null) return;

        // Envía si el personaje se está moviendo con las teclas
        animator.SetBool("Walking", input.sqrMagnitude > 0);

        // Envía si está tocando el suelo para controlar la animación de salto
        animator.SetBool("OnFloor", IsGrounded());
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
        if (isCrouching) return; // Evita saltar mientras se está agachada

        velocity += jumpForce;
    }

    public void Crouch(InputAction.CallbackContext context) // Maneja la acción de agacharse
    {
        if (context.started) // Al presionar la tecla
        {
            isCrouching = true;
            if (animator != null) animator.SetBool("Crouching", true);
            Debug.Log("Clementina se agachó");
        }
        else if (context.canceled) // Al soltar la tecla
        {
            isCrouching = false;
            if (animator != null) animator.SetBool("Crouching", false);
            Debug.Log("Clementina se levantó");
        }
    }

    public void Drink(InputAction.CallbackContext context) // Activa la animación de tomar una poción
    {
        if (!context.started) return;
        if (!IsGrounded()) return; // Evita tomar pociones en el aire

        if (animator != null) animator.SetTrigger("Drinking");
        Debug.Log("Clementina toma una poción");
    }

    public void GrabDown(InputAction.CallbackContext context) // Activa la animación de recoger del suelo
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        if (animator != null) animator.SetTrigger("GrabDown");
        Debug.Log("Recogiendo objeto del suelo");
    }

    public void GrabMid(InputAction.CallbackContext context) // Activa la animación de recoger de una mesa
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        if (animator != null) animator.SetTrigger("GrabMid");
        Debug.Log("Recogiendo objeto de la mesa/estante");
    }

    private bool IsGrounded() => characterController.isGrounded; // Comprueba si el jugador está en contacto con el suelo.
}
