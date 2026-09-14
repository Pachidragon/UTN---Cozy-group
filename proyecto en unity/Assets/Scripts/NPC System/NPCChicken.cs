using UnityEngine;

public class NPCChicken : NPCBase
{
    public override void Talk()
    {
        if (!FarmQuestManager.Instance.talkToFarmer)
        {
            Debug.Log($"[{nameNPC}]: ¿Quién eres tú?");
            return;
        }

        base.Talk();
    }
}
