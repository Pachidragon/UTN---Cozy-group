using UnityEngine;

// Activa/desactiva el control del jugador y el cursor cuando se abre una UI.
public class PlayerControlToggle : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private CameraManager cameraManager;

    public void OpenUI()
    {
        movement.enabled = false;
        cameraManager.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseUI()
    {
        movement.enabled = true;
        cameraManager.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
