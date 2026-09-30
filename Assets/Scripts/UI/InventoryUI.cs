using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractor player;
    [SerializeField] private Text inventoryText;

    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<PlayerInteractor>();
        if (inventoryText == null) inventoryText = GetComponent<Text>();
        Refresh();
    }

    private void Update() => Refresh();

    private void Refresh()
    {
        if (player == null || inventoryText == null) return;

        StringBuilder text = new StringBuilder();
        text.Append("📦 ").Append(player.CarriedItems).Append('/').Append(player.MaxCarryCapacity);

        foreach (ProductStack stack in player.Inventory)
            text.Append("\\n").Append(stack.productName).Append(": ").Append(stack.amount);

        inventoryText.text = text.ToString();
    }
}
