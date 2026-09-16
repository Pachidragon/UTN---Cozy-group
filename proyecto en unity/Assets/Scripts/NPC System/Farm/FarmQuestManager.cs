using UnityEngine;
// Gestiona el progreso de la misión de la granja y almacena sus estados.   
public class FarmQuestManager : MonoBehaviour
{
    public static FarmQuestManager Instance { get; private set; }
    public bool talkToFarmer = false; // Indica si el jugador ya habló con la granjera.
    public int chickensInterrogated = 0; // Cantidad de gallinas interrogadas por el jugador.
    public bool talkToHorse = false; // Indica si el jugador ya habló con el caballo.


    private void Awake() // Inicializa la instancia única del gestor de misión.
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
