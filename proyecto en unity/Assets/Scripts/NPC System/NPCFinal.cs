using UnityEngine;

public class NPCVictoria : NPCBase
{
    [SerializeField] private GameObject prefabItemFinal;
    [SerializeField] private Transform pivotSpawnItemFinal;

    private bool win = false;

    public override void Talk()
    {
        if (win) return;

        base.Talk();
        OtorgarVictoria();
    }

    private void OtorgarVictoria()
    {
        win = true;

        if (prefabItemFinal != null && pivotSpawnItemFinal != null)
        {
            Instantiate(prefabItemFinal, pivotSpawnItemFinal.position, pivotSpawnItemFinal.rotation);
        }

        if (GameCode.Instance != null)
        {
            GameCode.Instance.VictoryOn();
        }
    }
}