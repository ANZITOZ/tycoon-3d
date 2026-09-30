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

        int collected = machine.Collect(freeCapacity);
        return player.AddItems(collected);
    }

    public int GetStoredProducts()
    {
        return machine != null ? machine.StoredProducts : 0;
    }
}
