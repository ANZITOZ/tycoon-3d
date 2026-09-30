using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainSceneBootstrap : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<PlayerController>();
        EnsureEventSystem();
        MobileJoystick joystick = CreateJoystick();
        if (player != null) { player.SetJoystick(joystick); CreatePlayerVisual(); }
        CreateProductionMachine();
        CreateBuildingPlacementSystem();
        CreateInventoryUI();
        CreateMoneyUI();
        CreateSellZone();
        CreateUpgradeUI();
        CreateBuildMenuUI();
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null) return;
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }

    private void CreateBuildingPlacementSystem()
    {
        if (FindFirstObjectByType<BuildingPlacementSystem>() != null) return;
        new GameObject("Building Placement System", typeof(BuildingPlacementSystem));
    }

    private void CreatePlayerVisual()
    {
        if (player == null || player.transform.childCount > 0) return;
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visual.name = "Player Visual";
        visual.transform.SetParent(player.transform, false);
        visual.transform.localPosition = Vector3.zero;
        Collider visualCollider = visual.GetComponent<Collider>();
        if (visualCollider != null) Destroy(visualCollider);
        player.SetVisual(visual.transform);
    }

    private void CreateProductionMachine()
    {
        if (FindFirstObjectByType<ProductionMachine>() != null) return;
        GameObject machineObject = new GameObject("Production Machine");
        machineObject.transform.position = new Vector3(3f, 1f, 2f);
        machineObject.AddComponent<ProductionMachine>();
        machineObject.AddComponent<ProductionMachineVisual>();
        machineObject.AddComponent<ProductionMachineInteraction>();
    }

    private Canvas GetCanvas()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas != null) return canvas;
        GameObject canvasObject = new GameObject("Mobile UI", typeof(RectTransform));
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private void CreateInventoryUI()
    {
        Canvas canvas = GetCanvas();
        GameObject inventoryObject = new GameObject("Inventory UI", typeof(RectTransform), typeof(Text), typeof(InventoryUI));
        RectTransform inventoryRect = inventoryObject.GetComponent<RectTransform>();
        inventoryRect.SetParent(canvas.transform, false);
        inventoryRect.anchorMin = new Vector2(1f, 1f); inventoryRect.anchorMax = new Vector2(1f, 1f);
        inventoryRect.anchoredPosition = new Vector2(-120f, -60f); inventoryRect.sizeDelta = new Vector2(180f, 55f);
        Text inventoryText = inventoryObject.GetComponent<Text>();
        inventoryText.text = "📦 0/10"; inventoryText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        inventoryText.fontSize = 28; inventoryText.alignment = TextAnchor.MiddleCenter;

        GameObject buttonObject = new GameObject("Collect Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(InteractionButtonUI));
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>(); buttonRect.SetParent(canvas.transform, false);
        buttonRect.anchorMin = new Vector2(1f, .5f); buttonRect.anchorMax = new Vector2(1f, .5f);
        buttonRect.anchoredPosition = new Vector2(-130f, 0f); buttonRect.sizeDelta = new Vector2(220f, 80f);
        buttonObject.GetComponent<Image>().color = new Color(.1f, .65f, .25f, .95f);
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        RectTransform labelRect = labelObject.GetComponent<RectTransform>(); labelRect.SetParent(buttonRect, false);
        labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = Vector2.zero; labelRect.offsetMax = Vector2.zero;
        Text label = labelObject.GetComponent<Text>(); label.text = "COLETAR"; label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        label.fontSize = 24; label.alignment = TextAnchor.MiddleCenter;
    }

    private void CreateMoneyUI()
    {
        Canvas canvas = GetCanvas();
        GameObject moneyObject = new GameObject("Money UI", typeof(RectTransform), typeof(Text), typeof(MoneyUI));
        RectTransform rect = moneyObject.GetComponent<RectTransform>(); rect.SetParent(canvas.transform, false);
        rect.anchorMin = new Vector2(0f, 1f); rect.anchorMax = new Vector2(0f, 1f); rect.anchoredPosition = new Vector2(120f, -60f); rect.sizeDelta = new Vector2(220f, 55f);
        Text text = moneyObject.GetComponent<Text>(); text.text = "MONEY"; text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); text.fontSize = 28; text.alignment = TextAnchor.MiddleCenter;
    }

    private void CreateSellZone()
    {
        if (FindFirstObjectByType<SellZone>() != null) return;
        GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube); zone.name = "Sell Zone";
        zone.transform.position = new Vector3(-3f, .25f, 2f); zone.transform.localScale = new Vector3(2.5f, .5f, 2.5f);
        zone.AddComponent<SellZone>(); zone.AddComponent<SellZoneInteraction>();
        Canvas canvas = GetCanvas();
        GameObject button = new GameObject("Sell Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(SellButtonUI));
        RectTransform buttonRect = button.GetComponent<RectTransform>(); buttonRect.SetParent(canvas.transform, false);
        buttonRect.anchorMin = new Vector2(1f, .5f); buttonRect.anchorMax = new Vector2(1f, .5f); buttonRect.anchoredPosition = new Vector2(-130f, -100f); buttonRect.sizeDelta = new Vector2(220f, 80f);
        button.GetComponent<Image>().color = new Color(.15f, .45f, .8f, .95f);
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        RectTransform labelRect = labelObject.GetComponent<RectTransform>(); labelRect.SetParent(buttonRect, false); labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = Vector2.zero; labelRect.offsetMax = Vector2.zero;
        Text label = labelObject.GetComponent<Text>(); label.text = "VENDER"; label.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); label.fontSize = 24; label.alignment = TextAnchor.MiddleCenter;
    }

    private void CreateUpgradeUI()
    {
        Canvas canvas = GetCanvas();
        GameObject button = new GameObject("Upgrade Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(MachineUpgradeUI));
        RectTransform rect = button.GetComponent<RectTransform>(); rect.SetParent(canvas.transform, false); rect.anchorMin = new Vector2(1f, 0f); rect.anchorMax = new Vector2(1f, 0f); rect.anchoredPosition = new Vector2(-130f, 135f); rect.sizeDelta = new Vector2(220f, 70f);
        button.GetComponent<Image>().color = new Color(.75f, .5f, .1f, .95f);
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text)); RectTransform labelRect = labelObject.GetComponent<RectTransform>(); labelRect.SetParent(rect, false); labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = Vector2.zero; labelRect.offsetMax = Vector2.zero;
        Text label = labelObject.GetComponent<Text>(); label.text = "UPGRADE"; label.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); label.fontSize = 22; label.alignment = TextAnchor.MiddleCenter;
    }

    private void CreateBuildMenuUI()
    {
        Canvas canvas = GetCanvas();
        if (FindFirstObjectByType<BuildMenuUI>() != null) return;
        GameObject menuObject = new GameObject("Build Menu UI", typeof(RectTransform), typeof(BuildMenuUI));
        RectTransform rect = menuObject.GetComponent<RectTransform>(); rect.SetParent(canvas.transform, false); rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
    }

    private MobileJoystick CreateJoystick()
    {
        Canvas canvas = GetCanvas();
        GameObject backgroundObject = new GameObject("Joystick Background", typeof(RectTransform), typeof(Image), typeof(MobileJoystick));
        RectTransform background = backgroundObject.GetComponent<RectTransform>(); background.SetParent(canvas.transform, false); background.anchorMin = Vector2.zero; background.anchorMax = Vector2.zero; background.anchoredPosition = new Vector2(140f, 140f); background.sizeDelta = new Vector2(220f, 220f);
        backgroundObject.GetComponent<Image>().color = new Color(.15f, .15f, .15f, .7f);
        GameObject handleObject = new GameObject("Joystick Handle", typeof(RectTransform), typeof(Image));
        RectTransform handle = handleObject.GetComponent<RectTransform>(); handle.SetParent(background, false); handle.anchorMin = new Vector2(.5f, .5f); handle.anchorMax = new Vector2(.5f, .5f); handle.anchoredPosition = Vector2.zero; handle.sizeDelta = new Vector2(90f, 90f);
        handleObject.GetComponent<Image>().color = new Color(.85f, .85f, .85f, .95f);
        return backgroundObject.GetComponent<MobileJoystick>();
    }
}
