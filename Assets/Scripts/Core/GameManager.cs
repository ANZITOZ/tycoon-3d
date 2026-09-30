using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private PlayerProgression progression;
    [SerializeField] private MachineUnlockManager machineUnlockManager;

    public MoneyManager Money => moneyManager;
    public PlayerProgression Progression => progression;
    public MachineUnlockManager Machines => machineUnlockManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (moneyManager == null)
            moneyManager = FindFirstObjectByType<MoneyManager>();

        if (progression == null)
            progression = FindFirstObjectByType<PlayerProgression>();

        if (machineUnlockManager == null)
            machineUnlockManager = FindFirstObjectByType<MachineUnlockManager>();

        if (progression == null)
        {
            GameObject progressionObject = new GameObject("Player Progression");
            progression = progressionObject.AddComponent<PlayerProgression>();
        }

        if (machineUnlockManager == null)
        {
            GameObject machineManagerObject = new GameObject("Machine Unlock Manager");
            machineUnlockManager = machineManagerObject.AddComponent<MachineUnlockManager>();
        }
    }
}
