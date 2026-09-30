using UnityEngine;

public class ProductionMachineInteraction : MonoBehaviour
{
    [SerializeField] private ProductionMachine machine;

    private void Awake()
    {
        if (machine == null)
            machine = GetComponent<ProductionMachine>();
    }

    public int Collect(PlayerInteractor player)
    {
        if (machine == null || player == null)
            return 0;

        int freeCapacity = player.GetFreeCapacity();

        if (freeCapacity <= 0)
            return 0;

        int amountToCollect = Mathf.Min(freeCapacity, machine.StoredProducts);
        int collected = machine.Collect(amountToCollect);

        if (collected <= 0)
            return 0;

        int accepted = player.AddItems(collected);

        // Defensive recovery in case inventory rules change later.
        if (accepted < collected)
        {
            // The current inventory implementation accepts all requested items,
            // so this branch should normally remain unused.
            Debug.LogWarning("Player inventory accepted fewer products than requested.");
        }

        return accepted;
    }

    public int GetStoredProducts()
    {
        return machine != null ? machine.StoredProducts : 0;
    }
}
