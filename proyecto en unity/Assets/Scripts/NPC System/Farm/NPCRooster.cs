using UnityEngine;
// Controla la interacción con el gallo y la aparición de la hierba necesaria para la misión.

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

    public override void Talk() // Muestra el diálogo del gallo y, si se cumple la condición, hace aparecer la hierba.
    {
        
        if (FarmQuestManager.Instance != null && !FarmQuestManager.Instance.talkToHorse)
        {
            Debug.Log("[" + nameNPC + "]: ¡Qué bonita vista hay desde aquí arriba! ¡Ki-Kiri-Kí!");
            return;
        }

        base.Talk();

        if (itemTrueHerb != null)
        {
            itemTrueHerb.SetActive(true); // Hace aparecer la hierba en el techo.
            Debug.Log("¡La hierba ha aparecido en el techo!");
            this.enabled = false; // Desactiva este script para evitar repetir la interacción.
            }
    }
}

