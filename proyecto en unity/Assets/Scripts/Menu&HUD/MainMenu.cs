using UnityEngine;
using UnityEngine.SceneManagement;

// Controla las acciones principales del menú inicial.
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene; // Nombre de la escena que se cargará al comenzar el juego.

    [SerializeField] private string creditsScene; // Nombre de la escena de créditos.

    public void Play() // Carga la escena principal del juego.
    {
        if (!string.IsNullOrEmpty(gameScene))
        {
            SceneManager.LoadScene(gameScene);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena del juego no está asignado en el Inspector.");
        }
    }

    public void IrACreditos() //  Carga la escena de créditos.
    {
        if (!string.IsNullOrEmpty(creditsScene))
        {
            SceneManager.LoadScene(creditsScene);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena de créditos no está asignado en el Inspector.");
        }
    }

    public void Quit()  // Cierra la aplicación.
    {
        Debug.Log("Saliendo del juego...");

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}