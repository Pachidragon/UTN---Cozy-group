using UnityEngine;

public class NPRooster : NPCBase
{
    [SerializeField] private GameObject itemTrueHerb;

    void Start()
    {
        if (itemTrueHerb != null) itemTrueHerb.SetActive(false);
    }

    public override void Talk()
    {
        if (!FarmQuestManager.Instance.talkToHorse)
        {
            Debug.Log($"[{nameNPC}]: ¡Qué bonita vista hay desde aquí arriba! Ki-Kiri-Kí!");
            return;
        }

        base.Talk();

        if (itemTrueHerb != null)
        {
            itemTrueHerb.SetActive(true);
            Debug.Log("¡La hierba ha aparecido en el techo!");
        }
    }
}
