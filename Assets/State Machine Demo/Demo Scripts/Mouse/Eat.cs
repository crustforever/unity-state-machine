/// <summary>
///     state that controls how long the mouse eats and when to start following the cheese again
/// </summary>
public class Eat : EnumeratedState<Mouse.MouseState, MouseContext>
{
    private readonly float _cheese_eating_time;

    public Eat(float cheeseEatingTime)
    {
        this._cheese_eating_time = cheeseEatingTime;
    }

    public override void Update(float deltaTime)
    {
        //has the cheese been moved and have we finished eating?
        if (!this.Context.CanBiteCheese() && (this.Machine.ElapsedTimeInState >= this._cheese_eating_time))
        {
            //change state to begin following again
            this.Machine.ChangeState(Mouse.MouseState.FOLLOW);
        }
    }
}