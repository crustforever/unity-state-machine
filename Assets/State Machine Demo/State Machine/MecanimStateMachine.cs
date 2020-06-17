using System;
using UnityEngine;

/// <summary>
///     the same as EnumeratedStateMachine except it triggers a parameter on an Animator (of the same name as the new state) on each state change
/// </summary>
/// <typeparam name="TEnum">an enum that maps to your state objects (e.g. WALK, RUN, JUMP, FALL)</typeparam>
/// <typeparam name="TContext">an object to hold shared data between all states in the machine</typeparam>
public class MecanimStateMachine<TEnum, TContext> : EnumeratedStateMachine<TEnum, TContext> where TEnum : struct, IConvertible, IComparable, IFormattable
{
    //For reference on the crazy type constraints above see: http://www.growingwiththeweb.com/2013/02/using-enum-as-generic-type.html
    //Basically these constraints make sure the type parameter is an enum despite C# not letting us do that
    //It's a nice way of defining a set of states and I think its nice to work with despite its ugliness here
    //See Mouse.cs for the actual implementation

    private readonly Animator _animator;

    public MecanimStateMachine(Animator animator, TContext context, EnumeratedState<TEnum, TContext>[] enumeratedStates, TEnum initialState)
        : base(context, enumeratedStates, initialState)
    {
        this._animator = animator;
    }

    /// <summary>
    ///     changes the current state and triggers the mecanim state change
    /// </summary>
    public override void ChangeState(TEnum newState)
    {
        base.ChangeState(newState);
        this._animator.SetTrigger(this.CurrentState.ToString().ToLower());
    }
}