using UnityEngine;

public class BuildingPlacementSystem : MonoBehaviour
{
    public static BuildingPlacementSystem Instance { get; private set; }

    [SerializeField] private float gridSize = 1f;
    [SerializeField] private float buildHeight = 0.5f;

    private MachineDefinition selectedDefinition;
    private GameObject preview;
    private bool placing;
    private Vector3 lastPosition;
    private bool validPosition;

    public bool IsPlacing => placing;
    public MachineDefinition SelectedDefinition => selectedDefinition;
    public bool ValidPosition => validPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!placing)
            return;

        UpdatePreviewPosition();
    }

    public void BeginPlacement(MachineDefinition definition)
    {
        if (definition == null || GameManager.Instance == null || GameManager.Instance.Machines == null)
            return;

        if (!GameManager.Instance.Machines.IsUnlocked(definition))
            return;

        CancelPlacement();
        selectedDefinition = definition;
        placing = true;
        CreatePreview();
    }

    public void ConfirmPlacement()
    {
        if (!placing || selectedDefinition == null || !validPosition)
            return;

        GameObject machineObject = new GameObject(selectedDefinition.machineName);
        machineObject.transform.position = lastPosition;

        ProductionMachine machine = machineObject.AddComponent<ProductionMachine>();
        machine.Configure(selectedDefinition.productionTime, 5, selectedDefinition.productValue);
        machineObject.AddComponent<ProductionMachineVisual>();
        machineObject.AddComponent<ProductionMachineInteraction>();

        CancelPlacement();
    }

    public void CancelPlacement()
    {
        placing = false;
        selectedDefinition = null;
        validPosition = false;

        if (preview != null)
            Destroy(preview);

        preview = null;
    }

    private void CreatePreview()
    {
        preview = GameObject.CreatePrimitive(PrimitiveType.Cube);
        preview.name = "Building Preview";
        preview.transform.localScale = new Vector3(1.5f, 1f, 1.5f);

        Collider collider = preview.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);

        Renderer renderer = preview.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = new Color(0.2f, 0.8f, 0.3f, 0.55f);
            renderer.material = material;
        }
    }

    private void UpdatePreviewPosition()
    {
        Camera camera = Camera.main;
        if (camera == null || preview == null)
            return;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (!ground.Raycast(ray, out float distance))
            return;

        Vector3 position = ray.GetPoint(distance);
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        position.y = buildHeight;

        lastPosition = position;
        validPosition = IsValidBuildPosition(position);
        preview.transform.position = position;

        Renderer renderer = preview.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = validPosition
                ? new Color(0.2f, 0.8f, 0.3f, 0.55f)
                : new Color(0.9f, 0.2f, 0.2f, 0.55f);
    }

    private bool IsValidBuildPosition(Vector3 position)
    {
        Collider[] hits = Physics.OverlapBox(position, new Vector3(0.65f, 0.5f, 0.65f));

        foreach (Collider hit in hits)
        {
            if (hit == null || hit.transform == preview.transform)
                continue;

            if (hit.GetComponent<ProductionMachine>() != null)
                return false;
        }

        return true;
    }
}
