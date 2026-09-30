using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProductStack
{
    public string productName;
    public int unitValue;
    public int amount;

    public ProductStack(string name, int value, int count)
    {
        productName = name;
        unitValue = Mathf.Max(0, value);
        amount = Mathf.Max(0, count);
    }
}

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private int maxCarryCapacity = 10;
    [SerializeField] private float interactionRange = 2.5f;

    private readonly List<ProductStack> inventory = new List<ProductStack>();

    public int CarriedItems { get; private set; }
    public int MaxCarryCapacity => maxCarryCapacity;
    public float InteractionRange => interactionRange;
    public bool HasFreeCapacity => CarriedItems < maxCarryCapacity;
    public ProductionMachineInteraction NearbyMachine { get; private set; }
    public IReadOnlyList<ProductStack> Inventory => inventory;

    public event Action InventoryChanged;

    private void Update()
    {
        FindNearbyMachine();
    }

    private void FindNearbyMachine()
    {
        ProductionMachineInteraction[] machines = FindObjectsByType<ProductionMachineInteraction>(FindObjectsSortMode.None);
        ProductionMachineInteraction closest = null;
        float closestDistance = interactionRange;

        foreach (ProductionMachineInteraction machine in machines)
        {
            if (machine == null) continue;
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
        if (NearbyMachine == null) return 0;
        return NearbyMachine.Interact(this);
    }

    public int AddProduct(string productName, int unitValue, int amount)
    {
        if (string.IsNullOrWhiteSpace(productName) || amount <= 0) return 0;

        int accepted = Mathf.Min(amount, maxCarryCapacity - CarriedItems);
        if (accepted <= 0) return 0;

        ProductStack stack = inventory.Find(item => item.productName == productName);
        if (stack == null)
        {
            stack = new ProductStack(productName, unitValue, 0);
            inventory.Add(stack);
        }

        stack.unitValue = unitValue;
        stack.amount += accepted;
        CarriedItems += accepted;
        InventoryChanged?.Invoke();
        return accepted;
    }

    public int AddItems(int amount)
    {
        return AddProduct("Produto", 10, amount);
    }

    public int RemoveProduct(string productName, int amount)
    {
        if (string.IsNullOrWhiteSpace(productName) || amount <= 0) return 0;

        ProductStack stack = inventory.Find(item => item.productName == productName);
        if (stack == null) return 0;

        int removed = Mathf.Min(amount, stack.amount);
        stack.amount -= removed;
        CarriedItems -= removed;

        if (stack.amount <= 0)
            inventory.Remove(stack);

        InventoryChanged?.Invoke();
        return removed;
    }

    public int GetProductAmount(string productName)
    {
        ProductStack stack = inventory.Find(item => item.productName == productName);
        return stack != null ? stack.amount : 0;
    }

    public int RemoveAllItems()
    {
        int amount = CarriedItems;
        inventory.Clear();
        CarriedItems = 0;
        InventoryChanged?.Invoke();
        return amount;
    }

    public int GetInventoryValue()
    {
        int total = 0;
        foreach (ProductStack stack in inventory)
            total += stack.amount * stack.unitValue;
        return total;
    }

    public int GetFreeCapacity()
    {
        return maxCarryCapacity - CarriedItems;
    }
}
