using UnityEngine;
using UnityEngine.SceneManagement;
// Controla los estados de victoria y derrota, y la navegación entre escenas.
public class GameCode : MonoBehaviour
{
    public static GameCode Instance { get; private set; }

    [SerializeField] private GameObject losePanel; // Panel que se muestra al perder.
    [SerializeField] private GameObject victoryPanel;  // Panel que se muestra al ganar.

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() // Condiciones de inicio, paneles ocultos. 
    {
        if (losePanel != null) losePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void LoseOn() // Se ejecuta cuando el jugador pierde.
    {
        Debug.Log("Perdiste ¡Material equivocado!");
        ShowEndPanel(losePanel);
    }

    public void VictoryOn() // Se ejecuta cuando el jugador completa el juego.
    {
        Debug.Log("¡Completaste el juego!");
        ShowEndPanel(victoryPanel);
    }

    private void ShowEndPanel(GameObject panelAActivar) // Activa el panel correspondiente y pausa el juego
    {
        if (panelAActivar != null)
        {
            panelAActivar.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void RetryLevel() // Reinicia la escena actual para volver a jugar.
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()  // Regresa al menú principal.
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
