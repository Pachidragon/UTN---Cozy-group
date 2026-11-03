using UnityEngine;
using TMPro;

public class QuestTrackerUI : MonoBehaviour
{
    public static QuestTrackerUI Instance { get; private set; }

    [Header("Referencias de UI")]
    [SerializeField] private TMP_Text zoneTitleText;
    [SerializeField] private TMP_Text objectiveListText;

    [Header("Referencias de Escena (Zona Caldero)")]
    [SerializeField] private GameObject puertaNivelFinal;

    public enum GameZone { Tutorial, CalderoExtra, Hongos, Frutas, Cueva, Granja, Final }
    private GameZone currentZone = GameZone.Tutorial;

    private bool tutorialIngredientesCompletados = false;
    private bool tutorialPocionBebida = false;

    private bool calderoIngredientesCompletados = false;
    private bool calderoPocionBebida = false;
    private bool granjeraHabladaLocal = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeZone(GameZone.Tutorial);
    }

    public void ChangeZone(GameZone newZone)
    {
        currentZone = newZone;
        if (InventoryManager.Instance != null)
        {
            UpdateQuestPanel();
        }
    }

    public void RegistrarGranjeraHablada()
    {
        granjeraHabladaLocal = true;
        UpdateQuestPanel();
    }

    public void MarcarPocionBebida()
    {
        string escenaActual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (escenaActual.Contains("Tutorial") || currentZone == GameZone.Tutorial)
        {
            tutorialPocionBebida = true;
            Debug.Log("[Misiones]: Poción del tutorial completada.");
        }
        else
        {
            calderoPocionBebida = true;
            Debug.Log("[Misiones]: Poción final completada. Abriendo camino al ritual...");

            if (puertaNivelFinal != null)
            {
                puertaNivelFinal.SetActive(true);
            }

            UpdateQuestPanel();
        }
    }

    public void UpdateQuestPanel()
    {
        if (zoneTitleText == null || objectiveListText == null) return;

        string listaObjetivos = "";

        switch (currentZone)
        {
            case GameZone.Tutorial:
                zoneTitleText.text = "<b>ZONA TUTORIAL</b>";

                bool habloConMoria = NPCTutor.Instance != null && NPCTutor.Instance.currentState != NPCTutor.tutorial.Introduction;
                listaObjetivos += habloConMoria ? "<s>Habla con la profesora Casán</s>\n" : "Habla con la profesora Casán\n";

                if (NPCTutor.Instance != null && NPCTutor.Instance.currentState == NPCTutor.tutorial.TutorialDone)
                {
                    tutorialIngredientesCompletados = true;
                }
                int ingredientesEnMochila = ObtenerIngredientesTotalesEnInventario();
                if (ingredientesEnMochila >= 4) tutorialIngredientesCompletados = true;

                listaObjetivos += tutorialIngredientesCompletados ? "<s>Recoge los ingredientes (4/4)</s>\n" : $"Recoge los ingredientes ({ingredientesEnMochila}/4)\n";

                listaObjetivos += tutorialPocionBebida ? "<s>Haz la poción (Craftear y beber)</s>" : "Haz la poción (Craftear y beber)";
                break;

            case GameZone.CalderoExtra:
                zoneTitleText.text = "<b>ZONA CALDERO</b>";

                int ing1 = ObtenerCantidadItemReal("Funghi_01");
                int ing2 = ObtenerCantidadItemReal("Fruit_01");
                int ing3 = ObtenerCantidadItemReal("Crystal_01");
                int ing4 = ObtenerCantidadItemReal("Herb_01");

                int conseguidosCaldero = 0;
                if (ing1 > 0) conseguidosCaldero++;
                if (ing2 > 0) conseguidosCaldero++;
                if (ing3 > 0) conseguidosCaldero++;
                if (ing4 > 0) conseguidosCaldero++;

                if (conseguidosCaldero >= 4) calderoIngredientesCompletados = true;

                listaObjetivos += calderoIngredientesCompletados ? "<s>Recoge los 4 ingredientes (4/4)</s>\n" : $"Recoge los 4 ingredientes ({conseguidosCaldero}/4)\n";
                listaObjetivos += calderoPocionBebida ? "<s>Haz la poción final y bébela</s>" : "Haz la poción final y bébela";
                break;

            case GameZone.Hongos:
                zoneTitleText.text = "<b>ZONA HONGOS</b>";
                int hongosReales = ObtenerCantidadItemReal("Funghi_01");
                listaObjetivos += (hongosReales >= 4) ? "<s>Encuentra 4 hongos (4/4)</s>" : $"Encuentra 4 hongos ({hongosReales}/4)";
                break;

            case GameZone.Frutas:
                zoneTitleText.text = "<b>ZONA FRUTAS</b>";
                int frutasReales = ObtenerCantidadItemReal("Fruit_01");
                listaObjetivos += (frutasReales >= 2) ? "<s>Encuentra 2 frutas (2/2)</s>" : $"Encuentra 2 frutas ({frutasReales}/2)";
                break;

            case GameZone.Cueva:
                zoneTitleText.text = "<b>ZONA CUEVA</b>";
                int diamantesReales = ObtenerCantidadItemReal("Crystal_01");
                listaObjetivos += (diamantesReales >= 3) ? "<s>Encuentra 3 diamantes (3/3)</s>" : $"Encuentra 3 diamantes ({diamantesReales}/3)";
                break;

            case GameZone.Granja:
                zoneTitleText.text = "<b>ZONA GRANJA</b>";
                if (FarmQuestManager.Instance != null)
                {
                    bool granjeraSatisfecha = granjeraHabladaLocal || FarmQuestManager.Instance.talkToFarmer;
                    listaObjetivos += granjeraSatisfecha ? "<s>Investiga la granja (Habla con la granjera)</s>\n" : "Investiga la granja (Habla con la granjera)\n";

                    int progresoAnimales = 0;
                    if (granjeraSatisfecha) progresoAnimales = 2; // Gallinas interrogadas
                    if (FarmQuestManager.Instance.talkToHorse) progresoAnimales = 3; // Caballo interrogado

                    listaObjetivos += (progresoAnimales >= 3) ? "<s>Pregunta a los animales (3/3)</s>\n" : $"Pregunta a los animales ({progresoAnimales}/3)\n";

                    int tieneHierbaGallo = ObtenerCantidadItemReal("Herb_01");
                    bool consiguioHierba = tieneHierbaGallo > 0 || FarmQuestManager.Instance.questCompleted;
                    listaObjetivos += consiguioHierba ? "<s>Encuentra la hierba</s>" : "Encuentra la hierba";
                }
                break;

            case GameZone.Final:
                zoneTitleText.text = "<b>ZONA FINAL</b>";

                // Busca si el script final ya cambió su booleano a true
                NPCVictoria mirtha = FindFirstObjectByType<NPCVictoria>();
                bool finalHablo = mirtha != null && mirtha.gameObject.activeSelf && mirtha.GetType().GetField("terminoDeHablar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(mirtha) is bool b && b;

                if (mirtha != null && ObtenerCantidadItemReal("Yerba") > 0) finalHablo = true;

                listaObjetivos += finalHablo ? "<s>Habla con la profesora final</s>\n" : "Habla con la profesora final\n";

                int yerbaReal = ObtenerCantidadItemReal("Yerba");
                listaObjetivos += (yerbaReal > 0) ? "<s>Recoge la Yerba Mate</s>\n" : "Recoge la Yerba Mate\n";
                listaObjetivos += "Ceba el mate ceremonial";
                break;

        }

        objectiveListText.text = listaObjetivos;
    }

    private int ObtenerIngredientesTotalesEnInventario()
    {
        if (InventoryManager.Instance == null || InventoryManager.Instance.Slots == null) return 0;
        int total = 0;
        foreach (var slot in InventoryManager.Instance.Slots)
        {
            if (slot != null && slot.itemData != null)
            {
                total += (slot.quantity - slot.wrongQuantity);
            }
        }
        return total;
    }

    private int ObtenerCantidadItemReal(string itemIDBuscar)
    {
        if (InventoryManager.Instance == null || InventoryManager.Instance.Slots == null) return 0;

        int totalReal = 0;
        foreach (var slot in InventoryManager.Instance.Slots)
        {
            if (slot != null && slot.itemData != null && slot.itemData.itemID == itemIDBuscar)
            {
                int cantidadRealEnSlot = slot.quantity - slot.wrongQuantity;
                if (cantidadRealEnSlot > 0)
                {
                    totalReal += cantidadRealEnSlot;
                }
            }
        }
        return totalReal;
    }
}


