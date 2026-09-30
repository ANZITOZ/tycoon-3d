using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MachineUnlockState
{
    public string machineId;
    public bool unlocked;
}

public class MachineUnlockManager : MonoBehaviour
{
    [SerializeField] private List<MachineDefinition> machineDefinitions = new List<MachineDefinition>();
    [SerializeField] private List<MachineUnlockState> unlockStates = new List<MachineUnlockState>();

    public IReadOnlyList<MachineDefinition> MachineDefinitions => machineDefinitions;

    public event Action<MachineDefinition> MachineUnlocked;

    private void Start()
    {
        EnsureDefaultCatalog();
        EnsureStates();
    }

    private void EnsureDefaultCatalog()
    {
        if (machineDefinitions.Count > 0)
            return;

        machineDefinitions.Add(CreateRuntimeDefinition("basic_producer", "Produtor Básico", "None", "Produto Básico", 1, 0, 10, 3f));
        machineDefinitions.Add(CreateRuntimeDefinition("processor", "Processador", "Produto Básico", "Produto Processado", 2, 500, 30, 5f));
        machineDefinitions.Add(CreateRuntimeDefinition("advanced_factory", "Fábrica Avançada", "Produto Processado", "Produto Premium", 4, 2000, 90, 7f));
        machineDefinitions.Add(CreateRuntimeDefinition("packaging_factory", "Fábrica de Embalagem", "Produto Premium", "Produto de Luxo", 6, 7500, 220, 9f));
    }

    private MachineDefinition CreateRuntimeDefinition(
        string id,
        string name,
        string input,
        string output,
        int requiredLevel,
        int cost,
        int value,
        float productionTime)
    {
        MachineDefinition definition = ScriptableObject.CreateInstance<MachineDefinition>();
        definition.machineId = id;
        definition.machineName = name;
        definition.inputName = input;
        definition.outputName = output;
        definition.unlockLevel = requiredLevel;
        definition.unlockCost = cost;
        definition.productValue = value;
        definition.productionTime = productionTime;
        return definition;
    }

    private void EnsureStates()
    {
        foreach (MachineDefinition definition in machineDefinitions)
        {
            if (GetState(definition.machineId) != null)
                continue;

            unlockStates.Add(new MachineUnlockState
            {
                machineId = definition.machineId,
                unlocked = definition.unlockLevel <= 1 && definition.unlockCost <= 0
            });
        }
    }

    public bool IsUnlocked(MachineDefinition definition)
    {
        if (definition == null)
            return false;

        MachineUnlockState state = GetState(definition.machineId);
        return state != null && state.unlocked;
    }

    public bool CanUnlock(MachineDefinition definition)
    {
        if (definition == null || IsUnlocked(definition))
            return false;

        PlayerProgression progression = GameManager.Instance != null ? GameManager.Instance.Progression : null;
        MoneyManager money = GameManager.Instance != null ? GameManager.Instance.Money : null;

        return progression != null &&
               money != null &&
               progression.Level >= definition.unlockLevel &&
               money.CanAfford(definition.unlockCost);
    }

    public bool TryUnlock(MachineDefinition definition)
    {
        if (!CanUnlock(definition))
            return false;

        MoneyManager money = GameManager.Instance.Money;

        if (!money.Spend(definition.unlockCost))
            return false;

        MachineUnlockState state = GetState(definition.machineId);
        state.unlocked = true;

        GameManager.Instance.Progression.AddXP(Mathf.Max(10, definition.unlockLevel * 10));
        MachineUnlocked?.Invoke(definition);
        return true;
    }

    private MachineUnlockState GetState(string machineId)
    {
        return unlockStates.Find(state => state.machineId == machineId);
    }
}
