using UnityEngine;
using UnityEngine.UI;

public class SellButtonUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractor player;
    [SerializeField] private Button sellButton;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerInteractor>();

        if (sellButton == null)
            sellButton = GetComponent<Button>();

        if (buttonText == null)
        {
            Text[] texts = GetComponentsInChildren<Text>(true);
            if (texts.Length > 0)
                buttonText = texts[0];
        }

        sellButton.onClick.AddListener(Sell);
    }

    private void Update()
    {
        SellZoneInteraction zone = FindNearbySellZone();
        bool canSell = player != null && zone != null && player.CarriedItems > 0;

        sellButton.gameObject.SetActive(zone != null);
        sellButton.interactable = canSell;

        if (buttonText != null)
            buttonText.text = canSell ? "VENDER (" + player.CarriedItems + ")" : "VENDER";
    }

    private SellZoneInteraction FindNearbySellZone()
    {
        if (player == null)
            return null;

        SellZoneInteraction[] zones =
            FindObjectsByType<SellZoneInteraction>(FindObjectsSortMode.None);

        SellZoneInteraction closest = null;
        float closestDistance = 2.5f;

        foreach (SellZoneInteraction zone in zones)
        {
            if (zone == null)
                continue;

            float distance = Vector3.Distance(player.transform.position, zone.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closest = zone;
            }
        }

        return closest;
    }

    private void Sell()
    {
        SellZoneInteraction zone = FindNearbySellZone();

        if (zone != null && player != null)
            zone.Sell(player);
    }

    private void OnDestroy()
    {
        if (sellButton != null)
            sellButton.onClick.RemoveListener(Sell);
    }
}
