using UnityEngine;

public class NPCFarmer : NPCBase
{
    [SerializeField] private Sprite avatarFarmer;

    public override void Talk()
    {
        if (Dialogues.Instance == null || FarmQuestManager.Instance == null) return;

        if (FarmQuestManager.Instance.questCompleted)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Oh, veo que ya tienes la hierba mágica! Me alegra muchísimo que la encontraras, jovencita.", avatarFarmer, this);
            return;
        }
        Dialogues.Instance.ShowDialogue(nameNPC, "Esa hierba... ¿Dónde la había dejado? ¿La tenían las gallinas? Ay, mi cabeza no da para más hoy, ve a revisar qué te dicen.", avatarFarmer, this);

        FarmQuestManager.Instance.talkToFarmer = true;

        if (QuestTrackerUI.Instance != null)
        {
            QuestTrackerUI.Instance.RegistrarGranjeraHablada();
        }
    }
}

