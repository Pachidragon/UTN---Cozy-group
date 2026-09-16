using UnityEngine;
// Controla la interacción y recolección de objetos del escenario.
public class Recolectable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool wrongItem = false; // Indica si el objeto es un elemento incorrecto que provoca la derrota.


    public void Interact()
    {
        Debug.Log("Recolectado");

        if (wrongItem && GameCode.Instance != null) // Comprueba si el objeto es incorrecto y existe una instancia de GameCode.
        {
            GameCode.Instance.LoseOn(); // Activa el estado de derrota.
        }
        Destroy(gameObject); // Elimina el objeto de la escena.
    }
}
