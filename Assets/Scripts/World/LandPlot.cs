using UnityEngine;

public class LandPlot : MonoBehaviour
{
    [SerializeField] private int unlockCost = 500;
    [SerializeField] private bool unlocked;

    public int UnlockCost => unlockCost;
    public bool IsUnlocked => unlocked;

    public bool TryUnlock()
    {
        if (unlocked)
            return true;

        if (GameManager.Instance == null || GameManager.Instance.Money == null)
            return false;

        if (!GameManager.Instance.Money.CanAfford(unlockCost))
            return false;

        GameManager.Instance.Money.Spend(unlockCost);
        unlocked = true;

        RefreshVisual();
        return true;
    }

    private void Start()
    {
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.enabled = unlocked || !Application.isPlaying;
    }
}
