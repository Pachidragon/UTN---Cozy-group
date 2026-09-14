using UnityEngine;

public class NPCFarmer : NPCBase
{
    [SerializeField] private string[] dialoguesAfterHorse;

    public override void Talk()
    {
        if (FarmQuestManager.Instance.talkToHorse)
        {
            Debug.Log($"[{nameNPC}]: Estuve intentando recordar, pero aun no estoy segura. Pensaba que se la había dado a las gallinas, ¿Quizá me confundí...?");
        }
        else
        {
            base.Talk();
            FarmQuestManager.Instance.talkToFarmer = true;
        }
    }
}

