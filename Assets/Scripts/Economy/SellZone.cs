using UnityEngine;

public class SellZone : MonoBehaviour
{
    [SerializeField] private int productValue = 10;
    public int ProductValue => productValue;

    public int Sell(PlayerInteractor player)
    {
        if (player == null || player.CarriedItems <= 0 || GameManager.Instance == null)
            return 0;

        int items = player.RemoveAllItems();
        int earnings = items * productValue;

        if (GameManager.Instance.Money != null)
            GameManager.Instance.Money.Add(earnings);

        return earnings;
    }
}
