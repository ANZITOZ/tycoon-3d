using UnityEngine;

[CreateAssetMenu(menuName = "Tycoon/Machine Definition")]
public class MachineDefinition : ScriptableObject
{
    public string machineId = "machine";
    public string machineName = "Machine";
    public string inputName = "None";
    public string outputName = "Product";
    public int unlockLevel = 1;
    public int unlockCost = 0;
    public int productValue = 10;
    public float productionTime = 3f;
}
