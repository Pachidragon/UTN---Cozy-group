using UnityEngine;

public class NPCHorse : NPCBase
{
    public override void Talk()
    {
        if (!FarmQuestManager.Instance.talkToFarmer)
        {
            Debug.Log($"[{nameNPC}]: *Buffido* El granjero te busca, joven aprendiz.");
            return;
        }

        base.Talk();
        FarmQuestManager.Instance.talkToHorse = true;
    }
}
