using UnityEngine;
using UnityEngine.InputSystem;

public class NPCTutor : NPCBase
{
    public static NPCTutor Instance { get; private set; }

    [Header("Referencias del Juego")]
    [SerializeField] private GameObject doorPlace;
    [SerializeField] private Animator npcAnimator;

    [Header("Identidad Visual UI")]
    [SerializeField] private Sprite avatarNPC;

    public enum tutorial { Introduction, Potion, TutorialDone }
    public tutorial currentState = tutorial.Introduction;
    private bool yaSePresentoConClementina = false;
    private bool showingRecipe = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Limpieza total en el despertar del motor
        yaSePresentoConClementina = false;
        currentState = tutorial.Introduction;
    }

    protected override void Start()
    {
        base.Start(); // Ejecuta la búsqueda del jugador nativa de NPCBase

        // Confirmación en el inicio físico del nivel
        yaSePresentoConClementina = false;
        currentState = tutorial.Introduction;
        showingRecipe = false;

        PlayAnimation("Idle");
    }

    protected override void Update()
    {
        base.Update();

        if (showingRecipe && playerTransform != null)
        {
            float distancia = Vector3.Distance(transform.position, playerTransform.position);
            if (distancia <= distanciaParaHablar && Keyboard.current != null)
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame) PotionRecipe(1);
                if (Keyboard.current.digit2Key.wasPressedThisFrame) PotionRecipe(2);
                if (Keyboard.current.digit3Key.wasPressedThisFrame) PotionRecipe(3);
                if (Keyboard.current.digit4Key.wasPressedThisFrame) PotionRecipe(4);
            }
        }
    }

    public override void Talk()
    {
        if (Dialogues.Instance == null) return;
        PlayAnimation("Talk");
        if (!yaSePresentoConClementina)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Hola, querida! Hoy te enseñaré el arte de la alquimia básica. Prepara tu primera poción usando el caldero para demostrarme lo que vales.", avatarNPC, this);

            if (QuestTrackerUI.Instance != null)
            {
                QuestTrackerUI.Instance.UpdateQuestPanel();
            }

            Invoke(nameof(CambiarEstadoAPotionSeguro), 0.5f);

            Invoke(nameof(RegresarAIdle), 3f);
            return; // Corta por completo la ejecución del frame actual
        }

        // Lógica de diálogos recurrentes una vez terminada la presentación inicial
        switch (currentState)
        {
            case tutorial.Potion:
                Dialogues.Instance.ShowDialogue(nameNPC, "¿Qué esperas, mi amor? Ve al caldero y prepara la poción para poder continuar.", avatarNPC, this);
                Invoke(nameof(RegresarAIdle), 2f);
                break;

            case tutorial.TutorialDone:
                Dialogues.Instance.ShowDialogue(nameNPC, "¿Necesitas ayuda con los ingredientes de la receta? Dime cuál buscas:\n Bosque  Hongos  Cueva  Granja", avatarNPC, this);
                showingRecipe = true;
                break;
        }
    }

    // Método auxiliar para retrasar el cambio de estado de forma segura
    private void CambiarEstadoAPotionSeguro()
    {
        yaSePresentoConClementina = true; // Clausura la introducción definitiva
        currentState = tutorial.Potion;   // Libera la recolección física
        Debug.Log("[Tutorial]: Introducción finalizada con éxito. Recolección de ítems liberada.");
    }

    public void PotionCrafted()
    {
        if (Dialogues.Instance != null)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Excelente trabajo! La poción es perfecta. He desbloqueado la puerta hacia las zonas exteriores para que puedas continuar mi amor.", avatarNPC, this);

            if (doorPlace != null)
            {
                doorPlace.SetActive(false);
            }

            currentState = tutorial.TutorialDone;

            if (QuestTrackerUI.Instance != null)
            {
                QuestTrackerUI.Instance.UpdateQuestPanel();
            }

            Invoke(nameof(RegresarAIdle), 4f);
        }
    }

    private void PotionRecipe(int ingrediente)
    {
        if (Dialogues.Instance == null) return;
        PlayAnimation("Talk");

        switch (ingrediente)
        {
            case 1:
                Dialogues.Instance.ShowDialogue(nameNPC, "El Bosque está lleno de frutas, pero llegar hasta ellas es un gran desafío.", avatarNPC, this);
                break;
            case 2:
                Dialogues.Instance.ShowDialogue(nameNPC, "La Zona de Hongos es húmeda y mágica. Recuerda explorar con cautela.", avatarNPC, this);
                break;
            case 3:
                Dialogues.Instance.ShowDialogue(nameNPC, "La Cueva es oscura y misteriosa. Asegúrate de caminar con cuidado o podrías perderte.", avatarNPC, this);
                break;
            case 4:
                Dialogues.Instance.ShowDialogue(nameNPC, "En la Granja vive una granjera. Ten cuidado, sus animales ocultan secretos.", avatarNPC, this);
                break;
        }

        Invoke(nameof(RegresarAIdle), 3f);
    }

    private void PlayAnimation(string triggerName)
    {
        if (npcAnimator != null) npcAnimator.SetTrigger(triggerName);
    }

    private void RegresarAIdle()
    {
        PlayAnimation("Idle");
    }
}
