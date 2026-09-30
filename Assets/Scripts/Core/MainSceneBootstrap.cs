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
        CreateStartingLandPlot();
        CreateProductionMachine();
        CreateBuildingPlacementSystem();
        CreateInventoryUI(); CreateMoneyUI(); CreateSellZone(); CreateUpgradeUI(); CreateBuildMenuUI();
    }
    private void EnsureEventSystem() { if (EventSystem.current != null) return; GameObject e = new GameObject("EventSystem"); e.AddComponent<EventSystem>(); e.AddComponent<StandaloneInputModule>(); }
    private void CreateStartingLandPlot() { if (FindFirstObjectByType<LandPlot>() != null) return; GameObject plot = GameObject.CreatePrimitive(PrimitiveType.Cube); plot.name = "Starting Land Plot"; plot.transform.position = new Vector3(0f, -0.15f, 0f); plot.transform.localScale = new Vector3(10f, .3f, 10f); plot.AddComponent<LandPlot>(); }
    private void CreateBuildingPlacementSystem() { if (FindFirstObjectByType<BuildingPlacementSystem>() != null) return; new GameObject("Building Placement System", typeof(BuildingPlacementSystem)); }
    private void CreatePlayerVisual() { if (player == null || player.transform.childCount > 0) return; GameObject v = GameObject.CreatePrimitive(PrimitiveType.Capsule); v.name = "Player Visual"; v.transform.SetParent(player.transform, false); Collider c = v.GetComponent<Collider>(); if (c != null) Destroy(c); player.SetVisual(v.transform); }
    private void CreateProductionMachine() { if (FindFirstObjectByType<ProductionMachine>() != null) return; GameObject o = new GameObject("Production Machine"); o.transform.position = new Vector3(3f, 1f, 2f); ProductionMachine machine = o.AddComponent<ProductionMachine>(); machine.Configure(3f, 5, 10); machine.ConfigureChain("None", "Produto Básico", 1, 1, 10); o.AddComponent<ProductionMachineVisual>(); o.AddComponent<ProductionMachineInteraction>(); }
    private Canvas GetCanvas() { Canvas c = FindFirstObjectByType<Canvas>(); if (c != null) return c; GameObject o = new GameObject("Mobile UI", typeof(RectTransform)); c = o.AddComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; o.AddComponent<CanvasScaler>(); o.AddComponent<GraphicRaycaster>(); return c; }
    private void CreateInventoryUI() { Canvas c = GetCanvas(); GameObject o = new GameObject("Inventory UI", typeof(RectTransform), typeof(Text), typeof(InventoryUI)); RectTransform r=o.GetComponent<RectTransform>(); r.SetParent(c.transform,false); r.anchorMin=new Vector2(1,1); r.anchorMax=new Vector2(1,1); r.anchoredPosition=new Vector2(-120,-60); r.sizeDelta=new Vector2(180,55); Text t=o.GetComponent<Text>(); t.text="📦 0/10"; t.font=Resources.GetBuiltinResource<Font>("Arial.ttf"); t.fontSize=28; t.alignment=TextAnchor.MiddleCenter; GameObject b=new GameObject("Collect Button",typeof(RectTransform),typeof(Image),typeof(Button),typeof(InteractionButtonUI)); RectTransform br=b.GetComponent<RectTransform>(); br.SetParent(c.transform,false); br.anchorMin=new Vector2(1,.5f); br.anchorMax=new Vector2(1,.5f); br.anchoredPosition=new Vector2(-130,0); br.sizeDelta=new Vector2(220,80); b.GetComponent<Image>().color=new Color(.1f,.65f,.25f,.95f); }
    private void CreateMoneyUI() { Canvas c=GetCanvas(); GameObject o=new GameObject("Money UI",typeof(RectTransform),typeof(Text),typeof(MoneyUI)); RectTransform r=o.GetComponent<RectTransform>(); r.SetParent(c.transform,false); r.anchorMin=new Vector2(0,1); r.anchorMax=new Vector2(0,1); r.anchoredPosition=new Vector2(120,-60); r.sizeDelta=new Vector2(220,55); Text t=o.GetComponent<Text>(); t.font=Resources.GetBuiltinResource<Font>("Arial.ttf"); t.fontSize=28; t.alignment=TextAnchor.MiddleCenter; }
    private void CreateSellZone() { if(FindFirstObjectByType<SellZone>()!=null)return; GameObject z=GameObject.CreatePrimitive(PrimitiveType.Cube); z.name="Sell Zone"; z.transform.position=new Vector3(-3,.25f,2); z.transform.localScale=new Vector3(2.5f,.5f,2.5f); z.AddComponent<SellZone>(); z.AddComponent<SellZoneInteraction>(); }
    private void CreateUpgradeUI() { Canvas c=GetCanvas(); GameObject b=new GameObject("Upgrade Button",typeof(RectTransform),typeof(Image),typeof(Button),typeof(MachineUpgradeUI)); RectTransform r=b.GetComponent<RectTransform>(); r.SetParent(c.transform,false); r.anchorMin=new Vector2(1,0); r.anchorMax=new Vector2(1,0); r.anchoredPosition=new Vector2(-130,135); r.sizeDelta=new Vector2(220,70); }
    private void CreateBuildMenuUI() { Canvas c=GetCanvas(); if(FindFirstObjectByType<BuildMenuUI>()!=null)return; GameObject o=new GameObject("Build Menu UI",typeof(RectTransform),typeof(BuildMenuUI)); RectTransform r=o.GetComponent<RectTransform>(); r.SetParent(c.transform,false); r.anchorMin=Vector2.zero; r.anchorMax=Vector2.one; r.offsetMin=Vector2.zero; r.offsetMax=Vector2.zero; }
    private MobileJoystick CreateJoystick() { Canvas c=GetCanvas(); GameObject b=new GameObject("Joystick Background",typeof(RectTransform),typeof(Image),typeof(MobileJoystick)); RectTransform r=b.GetComponent<RectTransform>(); r.SetParent(c.transform,false); r.anchorMin=Vector2.zero; r.anchorMax=Vector2.zero; r.anchoredPosition=new Vector2(140,140); r.sizeDelta=new Vector2(220,220); GameObject h=new GameObject("Joystick Handle",typeof(RectTransform),typeof(Image)); RectTransform hr=h.GetComponent<RectTransform>(); hr.SetParent(r,false); hr.anchorMin=new Vector2(.5f,.5f); hr.anchorMax=new Vector2(.5f,.5f); hr.sizeDelta=new Vector2(90,90); return b.GetComponent<MobileJoystick>(); }
}
