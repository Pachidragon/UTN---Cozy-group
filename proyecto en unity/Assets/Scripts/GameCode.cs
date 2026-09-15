using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCode : MonoBehaviour
{
    public static GameCode Instance { get; private set; }

    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject victoryPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (losePanel != null) losePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void LoseOn()
    {
        Debug.Log("Perdiste ¡Material equivocado!");
        ShowEndPanel(losePanel);
    }

    public void VictoryOn()
    {
        Debug.Log("¡Completaste el juego!");
        ShowEndPanel(victoryPanel);
    }

    private void ShowEndPanel(GameObject panelAActivar)
    {
        if (panelAActivar != null)
        {
            panelAActivar.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
