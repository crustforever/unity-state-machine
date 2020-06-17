using UnityEngine;

/// <summary>
///     state that controls the position of the mouse while following the cheese
/// </summary>
public class Follow : EnumeratedState<Mouse.MouseState, MouseContext>
{
    private readonly float _follow_speed;

    public Follow(float followSpeed)
    {
        this._follow_speed = followSpeed;
    }

    public override void Update(float deltaTime)
    {
        //can we bite the cheese?
        if (this.Context.CanBiteCheese())
            //change state to begin eating
            this.Machine.ChangeState(Mouse.MouseState.EAT);
        else
        {
            //keep after it
            Vector2 velocity = this.Context.DirectionToCheese * this._follow_speed * deltaTime;
            this.Context.MouseTransform.position += new Vector3(velocity.x, velocity.y, 0.0f);
        }
    }
}