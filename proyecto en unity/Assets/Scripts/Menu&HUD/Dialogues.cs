using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogues : MonoBehaviour
{
    public static Dialogues Instance { get; private set; }

    [Header("Referencias de UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image avatarUI;
    [SerializeField] private TMP_Text nameUI;
    [SerializeField] private TMP_Text dialogueUI;

    public bool IsDialogueActive => dialoguePanel != null && dialoguePanel.activeSelf;

    // Almacena qué script de NPC tiene el control actual de la pantalla
    public NPCBase CurrentTalkingNPC { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowDialogue(string npcName, string text, Sprite npcAvatar, NPCBase callingNPC)
    {
        if (dialoguePanel == null) return;

        dialoguePanel.SetActive(true);
        nameUI.text = npcName;
        dialogueUI.text = text;

        // dueño actual del diálogo
        CurrentTalkingNPC = callingNPC;

        if (npcAvatar != null && avatarUI != null)
        {
            avatarUI.sprite = npcAvatar;
            avatarUI.enabled = true;
        }
        else if (avatarUI != null)
        {
            avatarUI.enabled = false;
        }
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        CurrentTalkingNPC = null; 
    }
}
