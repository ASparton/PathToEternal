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
    // Delegate indicating that the trigger has been triggered.
    public delegate void TriggerActivated();
    public event TriggerActivated TriggerActivatedEvent;

    // Delegate indicating that the trigger has been untriggered.
    public delegate void TriggerDeactivated();
    public event TriggerDeactivated TriggerDeactivatedEvent;

    #region Attributes

    [Header("Trigger")]
    [SerializeField][Tooltip("All the actor that have this tag will trigger the object when entering the cell.")]
    private List<string> _matchingTags;
    public List<string> MatchingTags { get { return _matchingTags; } }

    [SerializeField][Tooltip("True: Stay pressed forever when a dynamic actor has walked on it.\nFalse: Return to unpressed when a dynamic actor walks off.")]
    private bool _keepTriggered = false;
    public bool KeepTriggered { get { return _keepTriggered; } }

    private bool _isTriggered;
    public bool IsTriggered 
    {
        get { return _isTriggered; }
        set // Set the right aspect for the trigger and notify the mutable actor's that his trigger has been activated or deactivated
        {
            _isTriggered = value;
            SetAspect(_isTriggered);
            if (_isTriggered && TriggerActivatedEvent != null)
                TriggerActivatedEvent.Invoke();
            else if (!_isTriggered && TriggerDeactivatedEvent != null)
                TriggerDeactivatedEvent.Invoke();
        } 
    }

    #endregion

    /// <summary>
    /// Change the trigger's aspect in the game to its trigger state or not triggered depending on the given boolean.
    /// </summary>
    /// <param name="triggeredAspect">True to set the triggered aspect, false to set the not triggered aspect.</param>
    protected abstract void SetAspect(bool triggeredAspect);
}
