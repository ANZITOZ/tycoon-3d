using UnityEngine;

public class ProductionMachineInteraction : MonoBehaviour
{
    [SerializeField] private ProductionMachine machine;

    private void Awake()
    {
        if (machine == null) machine = GetComponent<ProductionMachine>();
    }

    public int Interact(PlayerInteractor player)
    {
        if (machine == null || player == null) return 0;

        // First priority: collect finished products.
        if (machine.StoredProducts > 0)
        {
            int freeCapacity = player.GetFreeCapacity();
            if (freeCapacity <= 0) return 0;

            int amountToCollect = Mathf.Min(freeCapacity, machine.StoredProducts);
            int collected = machine.Collect(amountToCollect);
            int accepted = player.AddProduct(machine.OutputName, machine.ProductValue, collected);
            return accepted;
        }

        // Second priority: supply the machine with the matching input.
        if (machine.RequiresInput)
        {
            int available = player.GetProductAmount(machine.InputName);
            if (available <= 0) return 0;

            int accepted = machine.AddInput(machine.InputName, available);
            if (accepted > 0)
                player.RemoveProduct(machine.InputName, accepted);
            return accepted;
        }

        return 0;
    }

    public int Collect(PlayerInteractor player)
    {
        return Interact(player);
    }

    public int GetStoredProducts()
    {
        return machine != null ? machine.StoredProducts : 0;
    }

    public string GetInteractionLabel(PlayerInteractor player)
    {
        if (machine == null) return "";
        if (machine.StoredProducts > 0) return "COLETAR " + machine.OutputName;
        if (machine.RequiresInput && player != null && player.GetProductAmount(machine.InputName) > 0)
            return "ABASTECER " + machine.InputName;
        if (machine.RequiresInput) return "PRECISA " + machine.InputName;
        return "PRODUZINDO";
    }
}
