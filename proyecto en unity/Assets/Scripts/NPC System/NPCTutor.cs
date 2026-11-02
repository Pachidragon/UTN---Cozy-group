using UnityEngine;
using UnityEngine.InputSystem;

// Controla el tutorial de pociones del NPC Tutor, la apertura de la puerta y la consulta posterior de ingredientes.
public class NPCTutor : NPCBase
{
    public static NPCTutor Instance { get; private set; }

    [Header("Configuración de Interacción")]
    [SerializeField] private float distanceToTalk = 4f;

    [Header("Referencias del Juego")]
    [SerializeField] private GameObject doorPlace; // Arrastra aquí la puerta que se abrirá
    [SerializeField] private Animator npcAnimator;       // Arrastra aquí el Animator del Tutor

    private Transform playerTransform;

    // Estados del Tutorial
    public enum tutorial { Introduction, Potion, TutorialDone }
    private tutorial currentState = tutorial.Introduction;

    private bool showingRecipe = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        PlayAnimation("Idle");
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanceToTalk)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }

            if (showingRecipe && Keyboard.current != null)
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame) PotionRecipe(1);
                if (Keyboard.current.digit2Key.wasPressedThisFrame) PotionRecipe(2);
                if (Keyboard.current.digit3Key.wasPressedThisFrame) PotionRecipe(3);
                if (Keyboard.current.digit4Key.wasPressedThisFrame) PotionRecipe(4);
            }
        }
        else
        {
            showingRecipe = false;
        }
    }

    public override void Talk()
    {
        PlayAnimation("Talk");

        switch (currentState)
        {
            case tutorial.Introduction:
                Debug.Log($"[{nameNPC}]: ¡Hola, querida! Hoy te enseñaré el arte de la alquimia básica.");
                Debug.Log($"[{nameNPC}]: Prepara tu primera poción usando el caldero para demostrarme lo que vales.");

                currentState = tutorial.Potion;
                Invoke(nameof(RegresarAIdle), 2f);
                break;

            case tutorial.Potion:
                Debug.Log($"[{nameNPC}]: ¿Qué esperas, mi amor? Ve al caldero y prepara la poción para poder continuar.");
                Invoke(nameof(RegresarAIdle), 2f);
                break;

            case tutorial.TutorialDone:
                Debug.Log($"[{nameNPC}]: ¿Necesitas ayuda con los ingredientes de la receta? Dime cuál buscas:");
                Debug.Log("[Presiona 1] El Bosque (Manzanas)");
                Debug.Log("[Presiona 2] La Zona de Hongos");
                Debug.Log("[Presiona 3] La Cueva (Cristales)");
                Debug.Log("[Presiona 4] La Granja (Secretos)");

                showingRecipe = true;
                break;
        }
    }

    // ESTA FUNCIÓN SE EJECUTARÁ AUTOMÁTICAMENTE CUANDO EL JUGADOR LOGRE EL CRAFTEO EXITOSO
    public void PotionCrafted()
    {
        if (currentState == tutorial.Potion)
        {
            Debug.Log($"[{nameNPC}]: ¡Excelente trabajo! La poción es perfecta. He desbloqueado la puerta hacia las zonas exteriores para que puedas continuar mi amor.");

            if (doorPlace != null)
            {
                doorPlace.SetActive(false); // Abre la puerta deshabilitando su objeto
                Debug.Log("[Sistema]: La puerta de la sala se ha abierto.");
            }

            currentState = tutorial.TutorialDone;
        }
    }

    private void PotionRecipe(int ingrediente)
    {
        PlayAnimation("Talk");

        switch (ingrediente)
        {
            case 1:
                Debug.Log($"[{nameNPC}]: El Bosque está lleno de frutas, pero llegar hasta ellas es un gran desafío.");
                break;
            case 2:
                Debug.Log($"[{nameNPC}]: La Zona de Hongos es húmeda y mágica. Recuerda explorar con cautela.");
                break;
            case 3:
                Debug.Log($"[{nameNPC}]: La Cueva es oscura y misteriosa. Asegúrate de caminar con cuidado o podrías perderte.");
                break;
            case 4:
                Debug.Log($"[{nameNPC}]: En la Granja vive una granjera. Ten cuidado, sus animales ocultan secretos.");
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
