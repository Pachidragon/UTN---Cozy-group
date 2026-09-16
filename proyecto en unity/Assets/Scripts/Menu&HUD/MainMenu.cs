using UnityEngine;
using UnityEngine.SceneManagement;
// Controla las acciones principales del menú inicial.

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene; // Nombre de la escena que se cargará al comenzar el juego. 
    public void Play() // Carga la escena principal del juego.
    {
        if (!string.IsNullOrEmpty(gameScene))
        {
            SceneManager.LoadScene(gameScene);
        }
    }

    public void Quit()  // Cierra la aplicación.
    {
        Application.Quit();
    }
}
