using UnityEngine;

public class FarmQuestManager : MonoBehaviour
{
    public static FarmQuestManager Instance { get; private set; }
    public bool talkToFarmer = false; // Indica si el jugador ya habló con la granjera.
    public int chickensInterrogated = 0; // Cantidad de gallinas interrogadas por el jugador.
    public bool talkToHorse = false; // Indica si el jugador ya habló con el caballo.
    public bool questCompleted = false; // Indica si la alumna ya recogió la hierba.

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        talkToFarmer = false;
        chickensInterrogated = 0;
        talkToHorse = false;
        questCompleted = false;
    }
}