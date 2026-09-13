using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactPivot;
    [SerializeField] private float interactionDistance;

    [SerializeField] private InputActionReference interactionReference;

    private void OnEnable()
    {
        interactionReference.action.Enable();
        interactionReference.action.started += PlayerInteracted;
    }

    private void OnDisable()
    {
        interactionReference.action.started -= PlayerInteracted;
        interactionReference.action.Disable();
    }

    private void PlayerInteracted(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Ray ray = new Ray(interactPivot.position, interactPivot.forward);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance)) return;

            if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
            {
                interactableObject.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(interactPivot.position, interactPivot.forward * interactionDistance);
    }
}
