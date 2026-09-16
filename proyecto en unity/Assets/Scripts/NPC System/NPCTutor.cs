using UnityEngine;
using UnityEngine.InputSystem;
// Controla el diálogo del NPC tutor (Moria) y permite al jugador consultar información sobre distintas zonas.

public class NPCTutor : NPCBase
{
    [SerializeField] private float distanceToTalk = 4f; // Distancia máxima a la que el jugador puede interactuar con el NPC.
    private Transform playerTransform;
    private bool options = false;

    void Start() // Busca y guarda la referencia al jugador al iniciar.
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update() // Comprueba la distancia al jugador y gestiona las entradas de interacción y selección de diálogo.
    {
        if (playerTransform == null) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanceToTalk)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }

            if (options && Keyboard.current != null)
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame) Reply(1);
                if (Keyboard.current.digit2Key.wasPressedThisFrame) Reply(2);
                if (Keyboard.current.digit3Key.wasPressedThisFrame) Reply(3);
                if (Keyboard.current.digit4Key.wasPressedThisFrame) Reply(4);
            }
        }
        else
        {
            options = false;
        }
    }

    public override void Talk() // Muestra el diálogo del tutor y las opciones de consulta disponibles.
    {
        Debug.Log("[" + nameNPC + "]: ¿Qué te pasa, mi amor? ¿En qué te ayuda esta deidad?");
        Debug.Log("[Presiona 1] El Bosque");
        Debug.Log("[Presiona 2] La Zona de Hongos");
        Debug.Log("[Presiona 3] La Cueva");
        Debug.Log("[Presiona 4] La Granja");

        options = true;
    }

    private void Reply(int opcionElegida) // Muestra la respuesta correspondiente a la opción seleccionada
    {
        switch (opcionElegida)
        {
            case 1:
                Debug.Log($"[{nameNPC}]: El Bosque está lleno de frutas, pero llegar hasta ellas es un gran desafío.");
                break;
            case 2:
                Debug.Log($"[{nameNPC}]: La Zona de Hongos es húmeda y mágica. Recuerda explorar con cautela.");
                break;
            case 3:
                Debug.Log($"[{nameNPC}]: La Cueva es oscura y misteriosa. Asegúrate de caminar con cuidado o podrías perderte.");
                break;
            case 4:
                Debug.Log($"[{nameNPC}]: En la Granja vive una granjera. Ten cuidado, sus animales ocultan secretos.");
                break;
        }

        options = false; // Cierra las opciones después de seleccionar una respuesta.
    }
}
