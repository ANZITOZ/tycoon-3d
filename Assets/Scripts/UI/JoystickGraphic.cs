using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class JoystickGraphic : Graphic
{
    [SerializeField, Range(4, 64)] private int segments = 32;
    [SerializeField] private float padding = 2f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect rect = rectTransform.rect;
        Vector2 center = rect.center;
        float radius = Mathf.Min(rect.width, rect.height) * 0.5f - padding;

        if (radius <= 0f)
            return;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = center;
        vh.AddVert(vertex);

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            vertex.position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            vh.AddVert(vertex);
        }

        for (int i = 0; i < segments; i++)
            vh.AddTriangle(0, i + 1, i + 2);
    }
}
