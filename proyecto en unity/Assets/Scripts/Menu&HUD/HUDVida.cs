using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDVida : MonoBehaviour
{
    public static HUDVida Instance { get; private set; }

    [Header("Componentes Visuales del HUD")]
    [SerializeField] private Image avatar; // Espacio para el avatar
    [SerializeField] private List<Image> heartsUI = new List<Image>(); // Lista para los corazones
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private int currentHearts = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHearts = 3;
        visualHearts();
    }

    // Disparador de daño
    public void GetDamage()
    {
        if (currentHearts <= 0) return;

        currentHearts--; // Restamos un corazón
        visualHearts();

        Debug.Log($"[HUD Vida]: ¡Explosión en el caldero! Poción fallida. Corazones restantes: {currentHearts}");

        // Si se queda sin vidas tras el tercer error, se activa la pantalla de derrota nativa
        if (currentHearts <= 0)
        {
            Debug.Log("[HUD Vida]: Clementina se quedó sin vidas. Activando derrota...");
            if (GameCode.Instance != null)
            {
                GameCode.Instance.LoseOn(); // Detona el panel
            }
        }
    }

    // Cambia los sprites de los corazones en base a la vida actual
    private void visualHearts()
    {
        for (int i = 0; i < heartsUI.Count; i++)
        {
            if (heartsUI[i] == null) continue;
            heartsUI[i].sprite = (i < currentHearts) ? fullHeart : emptyHeart;
        }
    }
}