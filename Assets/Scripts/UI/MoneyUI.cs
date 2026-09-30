using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Text moneyText;

    private void Start()
    {
        if (moneyManager == null && GameManager.Instance != null)
            moneyManager = GameManager.Instance.Money;

        if (moneyManager == null)
            moneyManager = FindFirstObjectByType<MoneyManager>();

        if (moneyText == null)
            moneyText = GetComponent<Text>();

        if (moneyManager != null)
            moneyManager.MoneyChanged += Refresh;

        Refresh();
    }

    private void Refresh()
    {
        if (moneyText != null && moneyManager != null)
            moneyText.text = "💰 $" + moneyManager.CurrentMoney;
    }

    private void OnDestroy()
    {
        if (moneyManager != null)
            moneyManager.MoneyChanged -= Refresh;
    }
}
