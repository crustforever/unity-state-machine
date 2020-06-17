using System;

/// <summary>
///     states used by an EnumeratedStateMachine inherit from this class
/// </summary>
/// <typeparam name="TEnum">an enum that maps to your state objects (e.g. WALK, RUN, JUMP, FALL)</typeparam>
/// <typeparam name="TContext">an object to hold shared data between all states in the machine</typeparam>
public abstract class EnumeratedState<TEnum, TContext> where TEnum : struct, IConvertible, IComparable, IFormattable
{
    //For reference on the crazy type constraints above see: http://www.growingwiththeweb.com/2013/02/using-enum-as-generic-type.html
    //Basically these constraints make sure the type parameter is an enum despite C# not letting us do that
    //It's a nice way of defining a set of states and I think its nice to work with despite its ugliness here
    //See Mouse.cs for the actual implementation

    protected TContext Context;
    protected EnumeratedStateMachine<TEnum, TContext> Machine;

    /// <summary>
    ///     sets references to the shared context object and the parent machine then calls OnInitialized
    /// </summary>
    /// <param name="machine"></param>
    /// <param name="context"></param>
    internal void SetMachineAndContext(EnumeratedStateMachine<TEnum, TContext> machine, TContext context)
    {
        this.Machine = machine;
        this.Context = context;
        OnInitialized();
    }

    /// <summary>
    ///     called directly after the machine and context are set allowing the state to do any required setup
    /// </summary>
    protected virtual void OnInitialized() {}

    /// <summary>
    ///     called directly before Begin(), only when this state is the initial state the machine enters
    /// </summary>
    public virtual void OnInitialState() {}

    /// <summary>
    ///     called whenever this state is entered
    /// </summary>
    public virtual void Begin() {}

    /// <summary>
    ///     called every tick when this is the current state
    /// </summary>
    public abstract void Update(float deltaTime);

    /// <summary>
    ///     called whenever this state is exited
    /// </summary>
    public virtual void End() {}
}