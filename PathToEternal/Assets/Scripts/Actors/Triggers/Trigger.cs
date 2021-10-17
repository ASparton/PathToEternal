using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A trigger is an Actor that has 2 states:
/// - Not triggered
/// - Triggered
/// And that can, if wanted, go back and forth between the two.
/// A Trigger runs an action to its bound Mutable object when triggered.
/// </summary>
public abstract class Trigger : Actor
{
    [Header("Trigger")]
    [SerializeField][Tooltip("All the actor that have this tag will trigger the object when entering the cell.")]
    private List<string> _matchingTags;
    public List<string> MatchingTags { get { return _matchingTags; } }

    [SerializeField][Tooltip("True: Stay pressed forever when a dynamic actor has walked on it.\nFalse: Return to unpressed when a dynamic actor walks off.")]
    private bool _keepTriggered = false;
    public bool KeepTriggered { get { return _keepTriggered; } }

    [SerializeField][Tooltip("The mutable actor that will execute its action when this trigger will be pressed.")]
    private MutableActor _mutableActor = null;
    public MutableActor MutableActor { get { return _mutableActor; } }

    private bool _isTriggered;
    public bool IsTriggered 
    {
        get { return _isTriggered; }
        set // Set the right aspect for the trigger and execute the mutable actor's trigger or untrigger action
        {
            _isTriggered = value;
            setAspect(_isTriggered);
            if (_isTriggered)
                _mutableActor.ExecuteTriggerAction(this);
            else
                _mutableActor.ExecuteUntriggerAction(this);
        } 
    }


    
    /// <summary>
    /// Change the trigger's aspect in the game to its trigger state or not triggered depending on the given boolean.
    /// </summary>
    /// <param name="triggeredAspect">True to set the triggered aspect, false to set the not triggered aspect.</param>
    protected abstract void setAspect(bool triggeredAspect);
}
