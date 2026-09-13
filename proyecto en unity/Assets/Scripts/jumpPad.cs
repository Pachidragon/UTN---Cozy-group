using UnityEngine;

public class jumpPad : MonoBehaviour
{

    [SerializeField] private float jumpForce;

    private void OnTriggerEnter(Collider other)

    { 
        
        Rigidbody rb = other.GetComponent <Rigidbody>();
        
        if (rb != null)
        {

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);


            rb.AddForce(Vector3.up* jumpForce, ForceMode.Impulse);
        }
    }

}