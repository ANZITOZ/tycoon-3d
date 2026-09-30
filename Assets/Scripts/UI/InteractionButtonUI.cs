using UnityEngine;
using UnityEngine.UI;

public class InteractionButtonUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractor player;
    [SerializeField] private Button collectButton;
    [SerializeField] private Text buttonText;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerInteractor>();

        if (collectButton == null)
            collectButton = GetComponent<Button>();

        if (buttonText == null)
        {
            Text[] texts = GetComponentsInChildren<Text>(true);
            if (texts.Length > 0)
                buttonText = texts[0];
        }

        collectButton.onClick.AddListener(Collect);
    }

    private void Update()
    {
        bool nearMachine = player != null && player.NearbyMachine != null;
        bool canCollect = nearMachine &&
                          player.NearbyMachine.GetStoredProducts() > 0 &&
                          player.HasFreeCapacity;

        collectButton.gameObject.SetActive(nearMachine);
        collectButton.interactable = canCollect;

        if (buttonText != null)
        {
            int stored = nearMachine ? player.NearbyMachine.GetStoredProducts() : 0;
            buttonText.text = stored > 0 ? $"COLETAR ({stored})" : "COLETAR";
        }
    }

    private void Collect()
    {
        if (player != null)
            player.CollectFromNearbyMachine();
    }

    private void OnDestroy()
    {
        if (collectButton != null)
            collectButton.onClick.RemoveListener(Collect);
    }
}
