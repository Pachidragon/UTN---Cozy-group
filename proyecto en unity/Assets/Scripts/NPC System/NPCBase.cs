using UnityEngine;

public abstract class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string nameNPC;
    [SerializeField] protected string[] dialogues;
    public virtual void Interact()
    {
        Talk();
    }

    public virtual void Talk()
    {
        if (dialogues != null && dialogues.Length > 0)
        {
            Debug.Log($"[{nameNPC}]: {dialogues[0]}");
        }
    }
}
