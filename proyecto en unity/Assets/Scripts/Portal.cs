using UnityEngine;
// Controla el teletransporte del jugador hacia un punto de destino.
public class Portal : MonoBehaviour
{
    public enum TipoPortal // Permite seleccionar el tipo de portal
    {
        Zone_Funghi,
        Zone_Farm,
        Zone_Cave,
        Zone_Fruits,
        Level_03
    }
    [SerializeField] private TipoPortal tipoPortal; // Clasificación del portal. Por ahora no modifica su comportamiento.


    public Transform destino; // Punto al que será teletransportado el jugador.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Comprueba si el objeto que entra al portal es el jugador.
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            controller.enabled = false;
            other.transform.position = destino.position;
            controller.enabled = true;
        }
    }
}
