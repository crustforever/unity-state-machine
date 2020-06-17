using UnityEngine;

/// <summary>
///     monobehavior that instantiates and updates the mouse state machine
/// </summary>
[RequireComponent(typeof(Animator))]
public class Mouse : MonoBehaviour
{
    #region MouseState enum

    public enum MouseState
    {
        FOLLOW,
        EAT
    }

    #endregion

    private SpriteRenderer _sprite_renderer; //store reference for convenience
    public float CheeseEatingRadius = 30.0f;
    public float CheeseEatingTime = 2.0f;
    public float FollowSpeed = 50.0f;
    public MecanimStateMachine<MouseState, MouseContext> MouseMachine;

    public void Awake()
    {
        this._sprite_renderer = GetComponent<SpriteRenderer>();

        //create the state machine!
        this.MouseMachine = new MecanimStateMachine<MouseState, MouseContext>(GetComponent<Animator>(),
            new MouseContext(this.transform, this.CheeseEatingRadius),
            new EnumeratedState<MouseState, MouseContext>[] {new Follow(this.FollowSpeed), new Eat(this.CheeseEatingTime)},
            MouseState.FOLLOW);
    }

    public void Update()
    {
        //always look to the cheese
        if (this.MouseMachine.Context.DirectionToCheese.x < 0)
            this._sprite_renderer.flipX = true;
        else
            this._sprite_renderer.flipX = false;

        //update the state machine!
        this.MouseMachine.Update(Time.deltaTime);
    }
}