using UnityEngine;

public class SellZoneInteraction : MonoBehaviour
{
    [SerializeField] private SellZone sellZone;

    private void Awake()
    {
        if (sellZone == null)
            sellZone = GetComponent<SellZone>();
    }

    public int Sell(PlayerInteractor player)
    {
        return sellZone != null ? sellZone.Sell(player) : 0;
    }
}
