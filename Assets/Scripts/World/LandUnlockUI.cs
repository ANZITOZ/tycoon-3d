using UnityEngine;
using UnityEngine.UI;

public class LandUnlockUI : MonoBehaviour
{
    [SerializeField] private LandPlot plot;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        if (plot == null)
            plot = FindFirstObjectByType<LandPlot>();

        if (button == null)
            button = GetComponent<Button>();

        if (buttonText == null)
        {
            Text[] texts = GetComponentsInChildren<Text>(true);
            if (texts.Length > 0)
                buttonText = texts[0];
        }

        button.onClick.AddListener(Unlock);
    }

    private void Update()
    {
        if (plot == null)
            return;

        bool affordable = GameManager.Instance != null &&
                          GameManager.Instance.Money != null &&
                          GameManager.Instance.Money.CanAfford(plot.UnlockCost);

        button.gameObject.SetActive(!plot.IsUnlocked);
        button.interactable = affordable;

        if (buttonText != null)
            buttonText.text = "DESBLOQUEAR $" + plot.UnlockCost;
    }

    private void Unlock()
    {
        if (plot != null)
            plot.TryUnlock();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(Unlock);
    }
}
