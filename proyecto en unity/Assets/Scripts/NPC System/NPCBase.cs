using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPCBase : MonoBehaviour, IInteractable
{
    [Header("Configuración Base NPC")]
    [SerializeField] protected string nameNPC; // Nombre del NPC
    [SerializeField] protected string[] dialogues; // Diálogos del Inspector
    [SerializeField] protected float distanciaParaHablar = 3f; // Distancia general

    protected Transform playerTransform;

    protected virtual void Start()
    {
        // Todos los NPCs buscan al jugador automáticamente al nacer
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    protected virtual void Update()
    {
        if (playerTransform == null) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanciaParaHablar)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
        else
        {
            if (Dialogues.Instance != null && Dialogues.Instance.IsDialogueActive && Dialogues.Instance.CurrentTalkingNPC == this)
            {
                Dialogues.Instance.CloseDialogue();
            }
        }
    }

    public virtual void Interact()
    {
        Talk();
    }

    public abstract void Talk();
}