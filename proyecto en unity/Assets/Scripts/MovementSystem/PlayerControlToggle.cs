using UnityEngine;

// Activa/desactiva el control del jugador y el cursor cuando se abre una UI.
public class PlayerControlToggle : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private PlayerInteractor interactor;

    public void OpenUI()
    {
        movement.enabled = false;
        cameraManager.enabled = false;
        if (interactor != null) interactor.SetBlocked(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseUI()
    {
        movement.enabled = true;
        cameraManager.enabled = true;
        if (interactor != null) interactor.SetBlocked(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
