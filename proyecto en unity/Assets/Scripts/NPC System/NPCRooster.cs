using UnityEngine;

public class NPCRooster : NPCBase
{
    [SerializeField] private GameObject itemTrueHerb;
    [SerializeField] private float distanciaParaHablar = 3f;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        if (itemTrueHerb != null) itemTrueHerb.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null) return;
        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanciaParaHablar)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }

    public override void Talk()
    {
        if (FarmQuestManager.Instance != null && !FarmQuestManager.Instance.talkToHorse)
        {
            Debug.Log("[" + nameNPC + "]: ¡Qué bonita vista hay desde aquí arriba! ¡Ki-Kiri-Kí!");
            return;
        }

        base.Talk();

        if (itemTrueHerb != null)
        {
            itemTrueHerb.SetActive(true);
            Debug.Log("¡La hierba ha aparecido en el techo!");
            this.enabled = false;
        }
    }
}

