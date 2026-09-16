using UnityEngine;
using UnityEngine.InputSystem;
// Gestiona la interacción del jugador con objetos interactuables mediante un Raycast.
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactPivot; // Punto desde donde se inicia la interacción.
    [SerializeField] private float interactionDistance; // Distancia máxima de interacción.

    [SerializeField] private InputActionReference interactionReference; // Acción de entrada utilizada para interactuar.

    private void OnEnable() // Habilita la acción de interacción y registra el método que la ejecuta.
    {
        if (interactionReference != null && interactionReference.action != null)
        {
            interactionReference.action.Enable();
            interactionReference.action.started += PlayerInteracted;
        }
    }

    private void OnDisable() // Deshabilita la acción de interacción y elimina el registro del método.
    {
        if (interactionReference != null && interactionReference.action != null)
        {
            interactionReference.action.started -= PlayerInteracted;
            interactionReference.action.Disable();
        }
    }

    private void PlayerInteracted(InputAction.CallbackContext context) // Detecta el objeto frente al jugador y ejecuta su interacción.
    {
        if (context.started)
        {
            Ray ray = new Ray(interactPivot.position, interactPivot.forward);

            int capaInteractuable = LayerMask.GetMask("Interactuable");

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, capaInteractuable)) return;

            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
            {
                interactableObject.Interact();
            }
        }
    }

    private void OnDrawGizmos() // Dibuja en la Scene View el alcance de la interacción del jugador.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(interactPivot.position, interactPivot.forward * interactionDistance);
    }
}
