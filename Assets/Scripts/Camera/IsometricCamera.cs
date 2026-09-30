using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(8f, 10f, -8f);
    [SerializeField] private float followSmoothTime = 0.12f;
    [SerializeField] private float lookHeight = 1f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            followSmoothTime);

        Vector3 lookTarget = target.position + Vector3.up * lookHeight;
        Quaternion targetRotation = Quaternion.LookRotation(
            lookTarget - transform.position,
            Vector3.up);

        transform.rotation = targetRotation;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
