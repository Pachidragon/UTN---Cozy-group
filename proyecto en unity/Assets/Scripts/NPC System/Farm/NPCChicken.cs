using UnityEngine;

public class NPCChicken : NPCBase
{
    [Header("Configuración de IA Autónoma")]
    [SerializeField] private float velocidadDeCaminata = 1.5f;
    [SerializeField] private float tiempoEntrePasos = 3f;
    [SerializeField] private float distanciaDeteccionValla = 1.2f;

    [Header("Identidad Visual UI")]
    [SerializeField] private Sprite avatarChicken;

    private Vector3 posicionInicial;
    private Vector3 direccionActual;
    private float temporizadorRuta;
    private bool alreadyTalked = false;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start(); // Busca al jugador en NPCBase
        posicionInicial = transform.position; // Guarda el centro del corral
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        ElegirNuevaDireccion();
    }

    protected override void Update()
    {
        base.Update();
        if (Dialogues.Instance != null && Dialogues.Instance.IsDialogueActive && Dialogues.Instance.CurrentTalkingNPC == this)
        {
            return;
        }

        transform.Translate(direccionActual * velocidadDeCaminata * Time.deltaTime, Space.World);

        if (direccionActual != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionActual);
            rotacionObjetivo *= Quaternion.Euler(0, -90, 0); // Compensación fija de Blender
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, 10f * Time.deltaTime);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, direccionActual, out hit, 1.4f))
        {
            if (hit.collider.CompareTag("Untagged") || hit.collider.name.Contains("fence") || hit.collider.name.Contains("valla") || hit.collider.name.Contains("Cerca"))
            {
                direccionActual = (posicionInicial - transform.position).normalized;
                temporizadorRuta = tiempoEntrePasos;
            }
        }

        // Límite de seguridad virtual por distancia al centro del corral
        if (Vector3.Distance(transform.position, posicionInicial) > 4.2f)
        {
            direccionActual = (posicionInicial - transform.position).normalized;
        }

        // Temporizador normal para cambiar de rumbo aleatorio
        temporizadorRuta -= Time.deltaTime;
        if (temporizadorRuta <= 0f)
        {
            ElegirNuevaDireccion();
        }
    }

    private void ElegirNuevaDireccion()
    {
        float angulo = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direccionActual = new Vector3(Mathf.Cos(angulo), 0f, Mathf.Sin(angulo)).normalized;

        // Límite del corral
        if (Vector3.Distance(transform.position, posicionInicial) > 4.5f)
        {
            direccionActual = (posicionInicial - transform.position).normalized;
        }

        temporizadorRuta = tiempoEntrePasos;
    }
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
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Cocorocó! ¿Quién eres tú? Habla primero con la granjera.", avatarChicken, this);
            return;
        }

        string textoAMostrar = (dialogues != null && dialogues.Length > 0) ? dialogues[0] : "¡Cocorocó! La hierba la tiene el caballo en el establo.";
        Dialogues.Instance.ShowDialogue(nameNPC, textoAMostrar, avatarChicken, this);
        if (!alreadyTalked)
        {
            alreadyTalked = true;
            FarmQuestManager.Instance.chickensInterrogated++;

            if (QuestTrackerUI.Instance != null) QuestTrackerUI.Instance.UpdateQuestPanel();
        }
    }
}



