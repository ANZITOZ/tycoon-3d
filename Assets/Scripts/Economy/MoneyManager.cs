using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingMoney = 100;

    public int CurrentMoney { get; private set; }

    public event Action<int> MoneyChanged;

    private void Awake()
    {
        CurrentMoney = startingMoney;
        MoneyChanged?.Invoke(CurrentMoney);
    }

    public bool CanAfford(int amount)
    {
        return amount >= 0 && CurrentMoney >= amount;
    }

    public bool Spend(int amount)
    {
        if (!CanAfford(amount))
            return false;

        CurrentMoney -= amount;
        MoneyChanged?.Invoke(CurrentMoney);
        return true;
    }

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        CurrentMoney += amount;
        MoneyChanged?.Invoke(CurrentMoney);
    }

    public void SetMoney(int amount)
    {
        CurrentMoney = Mathf.Max(0, amount);
        MoneyChanged?.Invoke(CurrentMoney);
    }
}
