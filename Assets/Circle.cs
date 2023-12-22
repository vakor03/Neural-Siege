using UnityEngine;

public struct Circle
{
    public Vector2 Center { get; set; }
    public float Radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public bool Intersects(Rect rect)
    {
        float closestX = Mathf.Clamp(Center.x, rect.x, rect.x + rect.width);
        float closestY = Mathf.Clamp(Center.y, rect.y, rect.y + rect.height);

        Vector2 closestPoint = new Vector2(closestX, closestY);
        float distance = Vector2.Distance(Center, closestPoint);

        return distance < Radius;
    }
}