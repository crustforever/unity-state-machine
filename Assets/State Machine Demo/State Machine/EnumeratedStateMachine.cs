using System;
using System.Collections.Generic;
using System.Globalization;
using Unitilities.Tuples;
using UnityEngine;

/// <summary>
///     a state machine that uses an enum to define its states
/// </summary>
/// <typeparam name="TEnum">an enum that maps to your state objects (e.g. WALK, RUN, JUMP, FALL)</typeparam>
/// <typeparam name="TContext">any type used to hold shared data between all states in the machine</typeparam>
public class EnumeratedStateMachine<TEnum, TContext> where TEnum : struct, IConvertible, IComparable, IFormattable
{
    //For reference on the crazy type constraints above see: http://www.growingwiththeweb.com/2013/02/using-enum-as-generic-type.html
    //Basically these constraints make sure the type parameter is an enum despite C# not letting us do that
    //It's a nice way of defining a set of states and I think its nice to work with despite its ugliness here
    //See Mouse.cs for the actual implementation

    public readonly TContext Context;
    private readonly EnumeratedState<TEnum, TContext>[] _states;

    //event that gets fired whenever the machine changes state
    public delegate void StateChangeAction(TEnum previousState, TEnum newState);
    public event StateChangeAction StateChangeEvent;

    //dictionary of events that get fired when a specific state transition occurs (after StateChangeEvent fires)
    public delegate void StateTransitionAction();
    public readonly Dictionary<Tuple<TEnum, TEnum>, StateTransitionAction> StateTransitionEvents;

    public float ElapsedTimeInState
    {
        get;
        private set;
    }

    public TEnum CurrentEnumeration
    {
        get;
        private set;
    }
    protected EnumeratedState<TEnum, TContext> CurrentState
    {
        get { return this._states[EnumeratedStateMachine<TEnum, TContext>.ToInt(this.CurrentEnumeration)]; }
    }

    public TEnum PreviousEnumeration
    {
        get;
        private set;
    }
    public EnumeratedState<TEnum, TContext> PreviousState
    {
        get { return this._states[EnumeratedStateMachine<TEnum, TContext>.ToInt(this.PreviousEnumeration)]; }
    }

    //constructor
    protected EnumeratedStateMachine(TContext context, EnumeratedState<TEnum, TContext>[] enumeratedStates, TEnum initialState)
    {
        if ((enumeratedStates.Length == 0) || (enumeratedStates.Length != Enum.GetValues(typeof(TEnum)).Length))
            throw new UnityException("Must have the same number of states as enum values!  Must have at least one state!");

        this.Context = context;
        this._states = enumeratedStates;
        foreach (EnumeratedState<TEnum, TContext> state in this._states)
        {
            state.SetMachineAndContext(this, context);
        }

        this.StateTransitionEvents = new Dictionary<Tuple<TEnum, TEnum>, StateTransitionAction>();
        this.CurrentEnumeration = initialState;
        this.CurrentState.OnInitialState();
        this.CurrentState.Begin();
    }

    /// <summary>
    ///     ticks the state machine with the provided delta time
    /// </summary>
    public void Update(float deltaTime)
    {
        this.ElapsedTimeInState += deltaTime;
        this._states[EnumeratedStateMachine<TEnum, TContext>.ToInt(this.CurrentEnumeration)].Update(deltaTime);
    }

    /// <summary>
    ///     changes the current state
    /// </summary>
    public virtual void ChangeState(TEnum newState)
    {
        //make sure that the value being passed in corresponds to an existing enum value
        if (!Enum.IsDefined(typeof(TEnum), newState))
            throw new UnityException("'" + newState + "' is not a valid member of the enum '" + typeof(TEnum).Name + "'!");

        //end the current state
        this.CurrentState.End();

        //fire state change events
        this.StateChangeEvent.Invoke(this.CurrentEnumeration, newState);

        //fire transition events if we have any
        if (this.StateTransitionEvents.ContainsKey(new Tuple<TEnum, TEnum>(this.CurrentEnumeration, newState)))
            this.StateTransitionEvents[new Tuple<TEnum, TEnum>(this.CurrentEnumeration, newState)].Invoke();

        //swap to new state
        this.PreviousEnumeration = this.CurrentEnumeration;
        this.CurrentEnumeration = newState;
        this.ElapsedTimeInState = 0.0f;

        //call Begin in the new state
        this.CurrentState.Begin();
    }

    /// <summary>
    ///     registers a state transition action. fired when state changes from the 'from' enumeration to the 'to' enumeration.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <param name="stateTransitionAction"></param>
    public void RegisterStateTransitionAction(TEnum to, TEnum from, StateTransitionAction stateTransitionAction)
    {
        Tuple<TEnum, TEnum> toFromTuple = new Tuple<TEnum, TEnum>(to, from);
        if (this.StateTransitionEvents.ContainsKey(toFromTuple))
            this.StateTransitionEvents[toFromTuple] += stateTransitionAction;
        else
            this.StateTransitionEvents.Add(toFromTuple, stateTransitionAction);
    }

    //HACK this is a bit of funkyness to convert our enum to an integer since technically we don't know this type param is an enum
    private static int ToInt(TEnum e)
    {
        return e.ToInt32(new CultureInfo("en-us"));
    }
}