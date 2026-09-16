using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
// Controla el menú de pausa y la navegación hacia el menú principal.
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // Panel visual que se muestra al pausar el juego.


    private bool isPaused = false; // Indica si el juego se encuentra pausado.

    void Update() // Detecta las teclas utilizadas para abrir o cerrar el menú de pausa.
    {
        if (Keyboard.current != null && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame() // Reanuda el juego y oculta el menú de pausa.
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame() // Pausa el juego y muestra el menú de pausa.
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu() // Regresa al menú principal y restablece el tiempo del juego.
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
