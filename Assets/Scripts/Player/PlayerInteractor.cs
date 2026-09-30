using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private int maxCarryCapacity = 10;

    public int CarriedItems { get; private set; }

    public bool HasFreeCapacity => CarriedItems < maxCarryCapacity;

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
