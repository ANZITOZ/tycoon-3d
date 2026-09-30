using System;
using UnityEngine;

public class ProductionMachine : MonoBehaviour
{
    [Header("Production Chain")]
    [SerializeField] private string inputName = "None";
    [SerializeField] private string outputName = "Produto";
    [SerializeField] private int inputRequired = 1;
    [SerializeField] private int outputAmount = 1;
    [SerializeField] private int productValue = 10;
    [SerializeField] private int inputStorageCapacity = 10;

    [Header("Production")]
    [SerializeField] private float productionInterval = 3f;
    [SerializeField] private int maxStorage = 5;

    [Header("Upgrade")]
    [SerializeField] private int level = 1;
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private float speedMultiplierPerLevel = 0.85f;
    [SerializeField] private int storageIncreasePerLevel = 2;
    [SerializeField] private int upgradeCostIncrease = 75;

    public int StoredProducts { get; private set; }
    public int StoredInput { get; private set; }
    public int MaxStorage => maxStorage;
    public int InputStorageCapacity => inputStorageCapacity;
    public int ProductValue => productValue;
    public float ProductionInterval => productionInterval;
    public int Level => level;
    public int UpgradeCost => upgradeCost;
    public bool IsFull => StoredProducts >= maxStorage;
    public string InputName => inputName;
    public string OutputName => outputName;
    public bool RequiresInput => !string.IsNullOrEmpty(inputName) && inputName != "None";
    public bool CanProduce => !IsFull && (!RequiresInput || StoredInput >= inputRequired);

    public event Action<int> StorageChanged;
    public event Action<int> InputChanged;
    public event Action OnProduced;
    public event Action OnUpgraded;

    private float productionTimer;

    public void Configure(float interval, int storage, int value)
    {
        productionInterval = Mathf.Max(0.1f, interval);
        maxStorage = Mathf.Max(1, storage);
        productValue = Mathf.Max(1, value);
    }

    public void ConfigureChain(string input, string output, int requiredInput, int amountProduced, int value)
    {
        inputName = string.IsNullOrEmpty(input) ? "None" : input;
        outputName = string.IsNullOrEmpty(output) ? "Produto" : output;
        inputRequired = Mathf.Max(1, requiredInput);
        outputAmount = Mathf.Max(1, amountProduced);
        productValue = Mathf.Max(1, value);
    }

    private void Update()
    {
        if (IsFull || !CanProduce)
        {
            productionTimer = 0f;
            return;
        }

        productionTimer += Time.deltaTime;
        if (productionTimer >= productionInterval)
        {
            productionTimer -= productionInterval;
            Produce();
        }
    }

    private void Produce()
    {
        if (!CanProduce) return;

        if (RequiresInput)
            StoredInput -= inputRequired;

        StoredProducts = Mathf.Min(maxStorage, StoredProducts + outputAmount);
        StorageChanged?.Invoke(StoredProducts);
        InputChanged?.Invoke(StoredInput);
        OnProduced?.Invoke();
    }

    public int AddInput(string productName, int amount)
    {
        if (!RequiresInput || productName != inputName || amount <= 0) return 0;

        int accepted = Mathf.Min(amount, inputStorageCapacity - StoredInput);
        if (accepted <= 0) return 0;

        StoredInput += accepted;
        productionTimer = 0f;
        InputChanged?.Invoke(StoredInput);
        return accepted;
    }

    public int Collect(int requestedAmount)
    {
        if (requestedAmount <= 0 || StoredProducts <= 0) return 0;
        int collected = Mathf.Min(requestedAmount, StoredProducts);
        StoredProducts -= collected;
        StorageChanged?.Invoke(StoredProducts);
        return collected;
    }

    public int CollectAll()
    {
        return Collect(StoredProducts);
    }

    public bool TryUpgrade()
    {
        if (GameManager.Instance == null || GameManager.Instance.Money == null) return false;
        if (!GameManager.Instance.Money.CanAfford(upgradeCost)) return false;

        GameManager.Instance.Money.Spend(upgradeCost);
        level++;
        productionInterval = Mathf.Max(0.5f, productionInterval * speedMultiplierPerLevel);
        maxStorage += storageIncreasePerLevel;
        upgradeCost += upgradeCostIncrease;
        OnUpgraded?.Invoke();
        StorageChanged?.Invoke(StoredProducts);
        return true;
    }

    public void UpgradeProduction(float newInterval, int newMaxStorage)
    {
        productionInterval = Mathf.Max(0.1f, newInterval);
        maxStorage = Mathf.Max(1, newMaxStorage);
        StoredProducts = Mathf.Min(StoredProducts, maxStorage);
        StorageChanged?.Invoke(StoredProducts);
    }
}
