using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    [SerializeField] private float jumpForce = 250f;
    [SerializeField] private float force = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>(); 
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        
        Debug.Log(input);
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0f, input.y) * force);
    }


    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rb.AddForce(Vector3.up * jumpForce);
            Debug.Log("Jump");
            
        }
        Debug.Log(callbackContext.phase);
    }

  
}
