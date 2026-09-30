using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private int maxCarryCapacity = 10;
    [SerializeField] private float interactionRange = 2.5f;

    public int CarriedItems { get; private set; }
    public int MaxCarryCapacity => maxCarryCapacity;
    public float InteractionRange => interactionRange;
    public bool HasFreeCapacity => CarriedItems < maxCarryCapacity;
    public ProductionMachineInteraction NearbyMachine { get; private set; }

    private void Update()
    {
        FindNearbyMachine();
    }

    private void FindNearbyMachine()
    {
        ProductionMachineInteraction[] machines =
            FindObjectsByType<ProductionMachineInteraction>(FindObjectsSortMode.None);

        ProductionMachineInteraction closest = null;
        float closestDistance = interactionRange;

        foreach (ProductionMachineInteraction machine in machines)
        {
            if (machine == null)
                continue;

            float distance = Vector3.Distance(transform.position, machine.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closest = machine;
            }
        }

        NearbyMachine = closest;
    }

    public int CollectFromNearbyMachine()
    {
        if (NearbyMachine == null)
            return 0;

        return NearbyMachine.Collect(this);
    }

    public int AddItems(int amount)
    {
        if (amount <= 0)
            return 0;

        int freeSpace = maxCarryCapacity - CarriedItems;
        int accepted = Mathf.Min(amount, freeSpace);

        CarriedItems += accepted;
        return accepted;
    }

    public int RemoveAllItems()
    {
        int amount = CarriedItems;
        CarriedItems = 0;
        return amount;
    }

    public int GetFreeCapacity()
    {
        return maxCarryCapacity - CarriedItems;
    }
}
