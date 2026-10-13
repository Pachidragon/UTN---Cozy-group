using UnityEngine;
using UnityEngine.InputSystem;

// Detecta objetos interactuables cercanos mediante una esfera y gestiona la interacción del jugador.
public class PlayerInteractor : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform interactPivot; // Punto desde donde se calcula la esfera.
    [SerializeField] private float interactionRadius; // Tamaño de la esfera.
    [SerializeField] private float forwardOffset; // Qué tan adelante del pivot se coloca la esfera.
    [SerializeField] private LayerMask interactableLayer; // Capa de los objetos interactuables.

    [Header("Input")]
    [SerializeField] private InputActionReference interactionReference; // Acción de entrada utilizada para interactuar.

    [Header("UI")]
    [SerializeField] private GameObject interactPrompt; // Cartel "E Interactuar".
    [SerializeField] private Camera cam; // Cámara que ve al jugador.
    [SerializeField] private Vector3 promptOffset = new Vector3(0f, 1f, 0f); // Qué tan arriba del objeto aparece el cartel.

    private IInteractable currentInteractable;
    private Transform currentTarget; // Transform del interactuable más cercano.

    private readonly Collider[] hits = new Collider[8]; // Buffer reutilizable para no generar basura cada frame.

    private void OnEnable()
    {
        if (interactionReference != null && interactionReference.action != null)
        {
            interactionReference.action.Enable();
            interactionReference.action.started += PlayerInteracted;
        }
    }

    private void OnDisable()
    {
        if (interactionReference != null && interactionReference.action != null)
        {
            interactionReference.action.started -= PlayerInteracted;
            interactionReference.action.Disable();
        }

        SetPromptVisible(false);
    }
    private void Update()
    {
        currentInteractable = FindClosestInteractable();

        if (currentTarget == null)
        {
            SetPromptVisible(false);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(currentTarget.position + promptOffset);

        // z < 0 significa que el objeto está detrás de la cámara y no se muestra el cartel.
        if (screenPos.z < 0f)
        {
            SetPromptVisible(false);
            return;
        }

        SetPromptVisible(true);
        interactPrompt.transform.position = screenPos;
    }

    private void PlayerInteracted(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    // Devuelve el interactuable más cercano dentro de la esfera, o null si no hay ninguno.
    private IInteractable FindClosestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(GetSphereCenter(), interactionRadius, hits, interactableLayer);

        IInteractable closest = null;
        Transform closestTransform = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            IInteractable interactable = hits[i].GetComponentInParent<IInteractable>();
            if (interactable == null) continue;

            float distance = (hits[i].transform.position - interactPivot.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
                Transform root = (interactable as Component).transform;
                Transform anchor = root.Find("PromptAnchor");
                closestTransform = anchor != null ? anchor : root;
            }
        }

        currentTarget = closestTransform;
        return closest;
    }

    private Vector3 GetSphereCenter() => interactPivot.position + interactPivot.forward * forwardOffset;

    private void SetPromptVisible(bool visible)
    {
        if (interactPrompt != null && interactPrompt.activeSelf != visible)
        {
            interactPrompt.SetActive(visible);
        }
    }

    private void OnDrawGizmos() // Dibuja en la Scene View el área de detección.
    {
        if (interactPivot == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetSphereCenter(), interactionRadius);
    }
}
