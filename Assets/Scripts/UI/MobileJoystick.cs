using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField, Range(0.1f, 1f)] private float handleRange = 0.6f;

    public Vector2 InputVector { get; private set; }

    private Canvas canvas;

    private void Awake()
    {
        if (background == null)
            background = transform as RectTransform;

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        Camera eventCamera = null;

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            eventCamera = canvas.worldCamera;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventCamera,
                out Vector2 localPoint))
        {
            return;
        }

        Vector2 radius = background.rect.size * 0.5f;
        Vector2 normalized = new Vector2(
            radius.x > 0 ? localPoint.x / radius.x : 0,
            radius.y > 0 ? localPoint.y / radius.y : 0);

        InputVector = Vector2.ClampMagnitude(normalized, 1f);

        handle.anchoredPosition = new Vector2(
            InputVector.x * radius.x * handleRange,
            InputVector.y * radius.y * handleRange);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
