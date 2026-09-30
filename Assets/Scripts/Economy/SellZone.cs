using UnityEngine;

public class SellZone : MonoBehaviour
{
    [SerializeField] private int productValue = 10;
    public int ProductValue => productValue;

    public int Sell(PlayerInteractor player)
    {
        if (player == null || player.CarriedItems <= 0 || GameManager.Instance == null || GameManager.Instance.Money == null)
            return 0;

        int earnings = player.GetInventoryValue();
        if (earnings <= 0) return 0;

        player.RemoveAllItems();
        GameManager.Instance.Money.Add(earnings);
        return earnings;
    }
}
