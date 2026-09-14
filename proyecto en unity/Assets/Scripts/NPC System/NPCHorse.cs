using UnityEngine;

public class NPCHorse : NPCBase
{
    [SerializeField] private float distanciaParaHablar = 3f;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
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
        if (FarmQuestManager.Instance != null && !FarmQuestManager.Instance.talkToFarmer)
        {
            Debug.Log("[" + nameNPC + "]: *Bufido* La granjera te busca, joven aprendiz.");
            return;
        }

        base.Talk();
        if (FarmQuestManager.Instance != null)
        {
            FarmQuestManager.Instance.talkToHorse = true;
        }
    }
}

