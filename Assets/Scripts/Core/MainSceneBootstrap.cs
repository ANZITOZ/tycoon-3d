using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainSceneBootstrap : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();

        EnsureEventSystem();
        MobileJoystick joystick = CreateJoystick();

        if (player != null)
        {
            player.SetJoystick(joystick);
            CreatePlayerVisual();
        }

        CreateProductionMachine();
        CreateInventoryUI();
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }

    private void CreatePlayerVisual()
    {
        if (player == null || player.transform.childCount > 0)
            return;

        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visual.name = "Player Visual";
        visual.transform.SetParent(player.transform, false);
        visual.transform.localPosition = Vector3.zero;

        Collider visualCollider = visual.GetComponent<Collider>();
        if (visualCollider != null)
            Destroy(visualCollider);

        player.SetVisual(visual.transform);
    }

    private void CreateProductionMachine()
    {
        if (FindFirstObjectByType<ProductionMachine>() != null)
            return;

        GameObject machineObject = new GameObject("Production Machine");
        machineObject.transform.position = new Vector3(3f, 1f, 2f);

        ProductionMachine machine = machineObject.AddComponent<ProductionMachine>();
        machineObject.AddComponent<ProductionMachineVisual>();
        machineObject.AddComponent<ProductionMachineInteraction>();
    }

    private void CreateInventoryUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Mobile UI", typeof(RectTransform));
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject inventoryObject = new GameObject("Inventory UI", typeof(RectTransform), typeof(Text), typeof(InventoryUI));
        RectTransform inventoryRect = inventoryObject.GetComponent<RectTransform>();
        inventoryRect.SetParent(canvas.transform, false);
        inventoryRect.anchorMin = new Vector2(1f, 1f);
        inventoryRect.anchorMax = new Vector2(1f, 1f);
        inventoryRect.anchoredPosition = new Vector2(-120f, -60f);
        inventoryRect.sizeDelta = new Vector2(180f, 55f);

        Text inventoryText = inventoryObject.GetComponent<Text>();
        inventoryText.text = "📦 0/10";
        inventoryText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        inventoryText.fontSize = 28;
        inventoryText.alignment = TextAnchor.MiddleCenter;

        GameObject buttonObject = new GameObject("Collect Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(InteractionButtonUI));
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.SetParent(canvas.transform, false);
        buttonRect.anchorMin = new Vector2(1f, 0.5f);
        buttonRect.anchorMax = new Vector2(1f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(-130f, 0f);
        buttonRect.sizeDelta = new Vector2(220f, 80f);

        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(0.1f, 0.65f, 0.25f, 0.95f);

        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.SetParent(buttonRect, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text label = labelObject.GetComponent<Text>();
        label.text = "COLETAR";
        label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        label.fontSize = 24;
        label.alignment = TextAnchor.MiddleCenter;

        buttonObject.GetComponent<InteractionButtonUI>();
    }

    private MobileJoystick CreateJoystick()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Mobile UI", typeof(RectTransform));
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject backgroundObject = new GameObject(
            "Joystick Background",
            typeof(RectTransform),
            typeof(Image),
            typeof(MobileJoystick));

        RectTransform background = backgroundObject.GetComponent<RectTransform>();
        background.SetParent(canvas.transform, false);
        background.anchorMin = new Vector2(0f, 0f);
        background.anchorMax = new Vector2(0f, 0f);
        background.anchoredPosition = new Vector2(140f, 140f);
        background.sizeDelta = new Vector2(220f, 220f);

        Image backgroundImage = backgroundObject.GetComponent<Image>();
        backgroundImage.color = new Color(0.15f, 0.15f, 0.15f, 0.7f);

        GameObject handleObject = new GameObject(
            "Joystick Handle",
            typeof(RectTransform),
            typeof(Image));

        RectTransform handle = handleObject.GetComponent<RectTransform>();
        handle.SetParent(background, false);
        handle.anchorMin = new Vector2(0.5f, 0.5f);
        handle.anchorMax = new Vector2(0.5f, 0.5f);
        handle.anchoredPosition = Vector2.zero;
        handle.sizeDelta = new Vector2(90f, 90f);

        Image handleImage = handleObject.GetComponent<Image>();
        handleImage.color = new Color(0.85f, 0.85f, 0.85f, 0.95f);

        return backgroundObject.GetComponent<MobileJoystick>();
    }
}
