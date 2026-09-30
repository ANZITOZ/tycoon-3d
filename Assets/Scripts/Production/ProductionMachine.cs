using System;
using UnityEngine;

public class ProductionMachine : MonoBehaviour
{
    [Header("Production")]
    [SerializeField] private float productionInterval = 3f;
    [SerializeField] private int maxStorage = 5;
    [SerializeField] private int productValue = 10;

    public int StoredProducts { get; private set; }
    public int MaxStorage => maxStorage;
    public int ProductValue => productValue;
    public float ProductionInterval => productionInterval;
    public bool IsFull => StoredProducts >= maxStorage;

    public event Action<int> StorageChanged;
    public event Action OnProduced;

    private float productionTimer;

    private void Update()
    {
        if (IsFull)
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
        if (StoredProducts >= maxStorage)
            return;

        StoredProducts++;
        StorageChanged?.Invoke(StoredProducts);
        OnProduced?.Invoke();
    }

    public int Collect(int requestedAmount)
    {
        if (requestedAmount <= 0 || StoredProducts <= 0)
            return 0;

        int collected = Mathf.Min(requestedAmount, StoredProducts);
        StoredProducts -= collected;

        StorageChanged?.Invoke(StoredProducts);
        return collected;
    }

    public int CollectAll()
    {
        return Collect(StoredProducts);
    }

    public void UpgradeProduction(float newInterval, int newMaxStorage)
    {
        productionInterval = Mathf.Max(0.1f, newInterval);
        maxStorage = Mathf.Max(1, newMaxStorage);
        StoredProducts = Mathf.Min(StoredProducts, maxStorage);
        StorageChanged?.Invoke(StoredProducts);
    }
}
