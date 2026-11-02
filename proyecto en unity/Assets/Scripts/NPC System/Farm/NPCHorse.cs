using UnityEngine;

public class NPCHorse : NPCBase
{
    [SerializeField] private Sprite avatarHorse;

    public override void Talk()
    {
        if (Dialogues.Instance == null || FarmQuestManager.Instance == null) return;

        if (FarmQuestManager.Instance.questCompleted)
        {
            Dialogues.Instance.CloseDialogue();
            return;
        }

        if (!FarmQuestManager.Instance.talkToFarmer)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "*Bufido* La granjera te busca, joven aprendiz.", avatarHorse, this);
            return;
        }

        // Muestra el secreto del corral de forma obligatoria
        string verdadCaballo = (dialogues != null && dialogues.Length > 0) ? dialogues[0] : "*Bufido* No les creas a las gallinas, jovencita. Te están mintiendo para quedarse con tu comida. El que sabe la verdad está en el techo...";
        Dialogues.Instance.ShowDialogue(nameNPC, verdadCaballo, avatarHorse, this);

        FarmQuestManager.Instance.talkToHorse = true; // Activa el pase para el gallo
    }
}


