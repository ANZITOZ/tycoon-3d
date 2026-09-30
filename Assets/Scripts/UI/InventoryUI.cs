using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractor player;
    [SerializeField] private Text inventoryText;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerInteractor>();

        if (inventoryText == null)
            inventoryText = GetComponent<Text>();

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (player == null || inventoryText == null)
            return;

        inventoryText.text = $"📦 {player.CarriedItems}/{player.MaxCarryCapacity}";
    }
}
