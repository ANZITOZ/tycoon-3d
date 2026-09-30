using UnityEngine;

public class ProductionChain : MonoBehaviour
{
    [SerializeField] private string inputName = "None";
    [SerializeField] private string outputName = "Product";
    [SerializeField] private int inputRequired = 1;
    [SerializeField] private int outputAmount = 1;
    [SerializeField] private int storageCapacity = 5;
    [SerializeField] private float productionTime = 3f;

    private int inputStored;
    private int outputStored;
    private float timer;

    public string InputName => inputName;
    public string OutputName => outputName;
    public int InputStored => inputStored;
    public int OutputStored => outputStored;
    public int StorageCapacity => storageCapacity;

    private void Update()
    {
        if (outputStored >= storageCapacity || inputStored < inputRequired)
            return;

        timer += Time.deltaTime;

        if (timer >= productionTime)
        {
            timer -= productionTime;
            inputStored -= inputRequired;
            outputStored += outputAmount;
        }
    }

    public int AddInput(int amount)
    {
        if (amount <= 0) return 0;

        int accepted = Mathf.Min(amount, 100 - inputStored);
        inputStored += accepted;
        return accepted;
    }

    public int CollectOutput(int amount)
    {
        if (amount <= 0) return 0;

        int collected = Mathf.Min(amount, outputStored);
        outputStored -= collected;
        return collected;
    }
}
