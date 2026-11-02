using UnityEngine;

public class NPCRooster : NPCBase
{
    [SerializeField] private GameObject itemTrueHerb;
    [SerializeField] private Sprite avatarRooster;

    protected override void Start()
    {
        base.Start();
        if (itemTrueHerb != null) itemTrueHerb.SetActive(false);
    }

    public override void Talk()
    {
        if (Dialogues.Instance == null || FarmQuestManager.Instance == null) return;

        if (!FarmQuestManager.Instance.talkToHorse)
        {
            string fraseTecho = (dialogues != null && dialogues.Length > 0) ? dialogues[0] : "¡Qué bonita vista hay desde aquí arriba! ¡Ki-Kiri-Kí!";
            Dialogues.Instance.ShowDialogue(nameNPC, fraseTecho, avatarRooster, this);
            return;
        }

        string confesion = (dialogues != null && dialogues.Length > 1) ? dialogues[1] : "¡Está bien, me descubriste! ¡Ki-kiri-kí! Yo me había llevado la hierba mágica al techo, aquí la tienes.";
        Dialogues.Instance.ShowDialogue(nameNPC, confesion, avatarRooster, this);

        // Hace aparecer el objeto físico en el juego para que el jugador lo agarre
        if (itemTrueHerb != null)
        {
            itemTrueHerb.SetActive(true);
            Debug.Log("[Sistema]: La hierba ha aparecido en el techo.");
        }

    }
}
