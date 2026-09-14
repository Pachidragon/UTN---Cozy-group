using UnityEngine;

public class platformMovement : MonoBehaviour
{
    [SerializeField] Vector3 direccionMovimiento = Vector3.up;
    [SerializeField] float distancia;
    [SerializeField] float velocidad;

    private Vector3 posicionInicial;
    private Rigidbody rb;

    private CharacterController jugadorCc;
    private Vector3 posicionPreviaPlataforma;

    void Start()
    {
        posicionInicial = transform.position;
        posicionPreviaPlataforma = transform.position;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        float factorMovimiento = Mathf.Sin(Time.time * velocidad);
        Vector3 nuevaPosicion = posicionInicial + (direccionMovimiento.normalized * factorMovimiento * (distancia / 2f));

        Vector3 desplazamiento = nuevaPosicion - posicionPreviaPlataforma;
        posicionPreviaPlataforma = nuevaPosicion;

        if (jugadorCc != null && jugadorCc.enabled)
        {
            jugadorCc.Move(desplazamiento);
        }

      
        rb.MovePosition(nuevaPosicion);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                jugadorCc = cc;  
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (jugadorCc != null && other.gameObject == jugadorCc.gameObject)
            {
                jugadorCc = null; 
                
            }
        }
    }
}