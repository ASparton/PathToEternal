using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MutableActor is an Actor that can change of aspect or do different actions.
/// Those actions are triggerd by one or multiple Trigger objects.
/// </summary>
public abstract class MutableActor : Actor
{
    [SerializeField][Tooltip("All the triggers that can make execute the action.")]
    private List<Trigger> _triggers;
    public List<Trigger> Triggers { get { return _triggers; } }

    /// <summary>
    /// Execute the mutable actor's trigger action.
    /// </summary>
    /// <param name="trigger">The Trigger that triggered the action.</param>
    public abstract void ExecuteTriggerAction(Trigger trigger);

    /// <summary>
    /// Execute the mutable actor's untrigger action.
    /// </summary>
    /// <param name="trigger">The Trigger that untriggered the action.</param>
    public abstract void ExecuteUntriggerAction(Trigger triger);
}
