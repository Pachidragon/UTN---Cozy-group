using UnityEngine;
// Controla el movimiento de una plataforma y permite que el jugador se desplace con ella.
public class platformMovement : MonoBehaviour
{
    [SerializeField] Vector3 direccionMovimiento = Vector3.up; // Dirección en la que se mueve la plataforma.
    [SerializeField] float distancia;  // Distancia total que recorre la plataforma.
    [SerializeField] float velocidad; // Velocidad del movimiento.

    private Vector3 posicionInicial;  // Posición de la plataforma al comenzar.
    private Rigidbody rb; // Rigidbody utilizado para mover la plataforma.

    private CharacterController jugadorCc;
    private Vector3 posicionPreviaPlataforma;

    void Start() // Guarda la posición inicial de la plataforma.
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

    private void OnTriggerEnter(Collider other) // Comprueba si el objeto que entra es el jugador.
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

    private void OnTriggerExit(Collider other) // Comprueba si el objeto que sale es el jugador.
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