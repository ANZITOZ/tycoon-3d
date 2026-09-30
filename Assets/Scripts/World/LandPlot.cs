using UnityEngine;

public class LandPlot : MonoBehaviour
{
    [SerializeField] private int unlockCost = 500;
    [SerializeField] private bool unlocked = true;
    [SerializeField] private Vector2 buildArea = new Vector2(5f, 5f);

    public int UnlockCost => unlockCost;
    public bool IsUnlocked => unlocked;
    public Vector2 BuildArea => buildArea;

    public bool ContainsBuildPosition(Vector3 position, float radius = 0.65f)
    {
        if (!unlocked) return false;
        Vector3 local = transform.InverseTransformPoint(position);
        return Mathf.Abs(local.x) <= buildArea.x * 0.5f - radius &&
               Mathf.Abs(local.z) <= buildArea.y * 0.5f - radius;
    }

    public bool TryUnlock()
    {
        if (unlocked) return true;
        if (GameManager.Instance == null || GameManager.Instance.Money == null) return false;
        if (!GameManager.Instance.Money.CanAfford(unlockCost)) return false;
        GameManager.Instance.Money.Spend(unlockCost);
        unlocked = true;
        RefreshVisual();
        return true;
    }

    private void Start() => RefreshVisual();

    private void RefreshVisual()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = true;
    }
}
