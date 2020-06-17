using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     display the current state of the mouse and how many cheeses its eaten in the UI
/// </summary>
public class MouseTracker : MonoBehaviour
{
    public Mouse Mouse;

    private int _cheeses_eaten;
    private int CheesesEaten
    {
        get { return this._cheeses_eaten; }
        set
        {
            this._cheeses_eaten = value;
            this.transform.Find("cheese counter").GetComponent<Text>().text = "cheeses eaten: " + this._cheeses_eaten;
        }
    }

    private Mouse.MouseState _current_state;
    private Mouse.MouseState CurrentState
    {
        set
        {
            this._current_state = value;
            this.transform.Find("state").GetComponent<Text>().text = "current state: " + this._current_state;
        }
    }

    public void Start()
    {
        this.CheesesEaten = 0;
        this.Mouse.MouseMachine.RegisterStateTransitionAction(Mouse.MouseState.EAT, Mouse.MouseState.FOLLOW, () => this.CheesesEaten++);
        this.CurrentState = this.Mouse.MouseMachine.CurrentEnumeration;
        this.Mouse.MouseMachine.StateChangeEvent += (previous, current) => this.CurrentState = current;
    }
}