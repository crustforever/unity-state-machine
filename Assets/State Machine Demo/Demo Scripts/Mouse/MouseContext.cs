using UnityEngine;

/// <summary>
///     api for shared data that describes the mouse
/// </summary>
public class MouseContext
{
    private readonly float _cheese_eating_radius;
    public readonly Transform MouseTransform;

    public MouseContext(Transform mouseTransform, float cheeseEatingRadius)
    {
        this.MouseTransform = mouseTransform;
        this._cheese_eating_radius = cheeseEatingRadius;
    }

    private static Vector2 PositionOfCheese
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition).xy(); }
    }

    private float DistanceToCheese
    {
        get { return Vector2.Distance(this.MouseTransform.position.xy(), MouseContext.PositionOfCheese); }
    }

    public Vector2 DirectionToCheese
    {
        get { return (MouseContext.PositionOfCheese - this.MouseTransform.position.xy()).normalized; }
    }

    public bool CanBiteCheese()
    {
        return this.DistanceToCheese <= this._cheese_eating_radius;
    }
}