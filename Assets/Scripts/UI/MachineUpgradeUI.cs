using UnityEngine;
using UnityEngine.UI;

public class MachineUpgradeUI : MonoBehaviour
{
    [SerializeField] private ProductionMachine machine;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        if (machine == null)
            machine = FindFirstObjectByType<ProductionMachine>();

        if (upgradeButton == null)
            upgradeButton = GetComponent<Button>();

        if (buttonText == null)
        {
            Text[] texts = GetComponentsInChildren<Text>(true);
            if (texts.Length > 0) buttonText = texts[0];
        }

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        if (machine == null || upgradeButton == null)
            return;

        bool affordable = GameManager.Instance != null &&
                          GameManager.Instance.Money != null &&
                          GameManager.Instance.Money.CanAfford(machine.UpgradeCost);

        upgradeButton.interactable = affordable;
        if (buttonText != null)
            buttonText.text = "UPGRADE L" + machine.Level + "  $" + machine.UpgradeCost;
    }

    private void Upgrade()
    {
        if (machine != null)
            machine.TryUpgrade();
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(Upgrade);
    }
}
