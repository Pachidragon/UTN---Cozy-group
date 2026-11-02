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

    // Usamos override para sumar lo nuestro al Start de la clase base
    protected override void Start()
    {
        base.Start(); // Ejecuta la búsqueda del jugador que está en NPCBase
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

        switch (currentState)
        {
            case tutorial.Introduction:
                Dialogues.Instance.ShowDialogue(nameNPC, "¡Hola, querida! Hoy te enseñaré el arte de la alquimia básica. Prepara tu primera poción usando el caldero para demostrarme lo que vales.", avatarNPC, this);
                currentState = tutorial.Potion;
                Invoke(nameof(RegresarAIdle), 3f);
                break;

            case tutorial.Potion:
                Dialogues.Instance.ShowDialogue(nameNPC, "¿Qué esperas, mi amor? Ve al caldero y prepara la poción para poder continuar.", avatarNPC, this);
                Invoke(nameof(RegresarAIdle), 2f);
                break;

            case tutorial.TutorialDone:
                Dialogues.Instance.ShowDialogue(nameNPC, "¿Necesitas ayuda con los ingredientes de la receta? Dime cuál buscas:\n[1] Bosque  [2] Hongos  [3] Cueva  [4] Granja", avatarNPC, this);
                showingRecipe = true;
                break;
        }
    }

    public void PotionCrafted()
    {
        if (currentState == tutorial.Potion && Dialogues.Instance != null)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Excelente trabajo! La poción es perfecta. He desbloqueado la puerta hacia las zonas exteriores para que puedas continuar mi amor.", avatarNPC, this);

            if (doorPlace != null)
            {
                doorPlace.SetActive(false);
            }

            currentState = tutorial.TutorialDone;
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
