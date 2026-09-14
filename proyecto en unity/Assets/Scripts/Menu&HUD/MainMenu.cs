using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene;
    public void Play()
    {
        if (!string.IsNullOrEmpty(gameScene))
        {
            SceneManager.LoadScene(gameScene);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
