using UnityEngine;
// Define el comportamiento base de los NPC que pueden interactuar con el jugador.

public abstract class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string nameNPC; // Nombre del NPC que se muestra en los diálogos.
    [SerializeField] protected string[] dialogues; // Lista de diálogos configurables desde el Inspector.

    public virtual void Interact() // Ejecuta la interacción del NPC con el jugador.
    {
        Talk();
    }

    public virtual void Talk() // Muestra el primer diálogo configurado del NPC.
    {
        if (dialogues != null && dialogues.Length > 0)
        {
            Debug.Log("[" + nameNPC + "]: " + dialogues[0]);
        }
        else
        {
            Debug.Log("[" + nameNPC + "]: No tengo diálogos configurados en el Inspector.");
        }
    }
}
