using UnityEngine;

public class FarmQuestManager : MonoBehaviour
{
    public static FarmQuestManager Instance { get; private set; }
    public bool talkToFarmer = false;
    public int chickensInterrogated = 0;
    public bool talkToHorse = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
