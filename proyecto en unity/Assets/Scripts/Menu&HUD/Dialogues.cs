using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogues: MonoBehaviour
{
    public static Dialogues Instance { get; private set; }

    [Header("Referencias de UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image avatarUI;
    [SerializeField] private TMP_Text nameUI;
    [SerializeField] private TMP_Text dialogueUI;

    public bool IsDialogueActive => dialoguePanel != null && dialoguePanel.activeSelf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Muestra el cuadro en pantalla con los datos del NPC activo
    public void ShowDialogue(string npcName, string text, Sprite npcAvatar)
    {
        if (dialoguePanel == null) return;

        dialoguePanel.SetActive(true);
        nameUI.text = npcName;
        dialogueUI.text = text;

        if (npcAvatar != null && avatarUI != null)
        {
            avatarUI.sprite = npcAvatar;
            avatarUI.enabled = true;
        }
        else if (avatarUI != null)
        {
            avatarUI.enabled = false; // Oculta el cuadro si el NPC no tiene foto
        }
    }

    // Cierra el panel de forma visual
    public void CloseDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}