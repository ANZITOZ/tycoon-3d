using UnityEngine;

public class ProductionMachineVisual : MonoBehaviour
{
    [SerializeField] private ProductionMachine machine;

    [Header("Visual")]
    [SerializeField] private Vector3 machineScale = new Vector3(2f, 1.5f, 2f);

    private GameObject visual;

    private void Start()
    {
        if (machine == null)
            machine = GetComponent<ProductionMachine>();

        CreateVisual();

        if (machine != null)
            machine.StorageChanged += OnStorageChanged;
    }

    private void OnDestroy()
    {
        if (machine != null)
            machine.StorageChanged -= OnStorageChanged;
    }

    private void CreateVisual()
    {
        if (visual != null)
            return;

        visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visual.name = "Machine Visual";
        visual.transform.SetParent(transform, false);
        visual.transform.localScale = machineScale;

        Collider collider = visual.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);
    }

    private void OnStorageChanged(int stored)
    {
        // Placeholder visual feedback.
        // Later this will show product models, lights and production animations.
        if (visual != null)
            visual.transform.localScale = machineScale * (1f + stored * 0.015f);
    }
}
