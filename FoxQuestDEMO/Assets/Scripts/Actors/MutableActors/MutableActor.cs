using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MutableActor is an Actor that can change of aspect or do different actions.
/// Those actions are triggerd by one or multiple Trigger objects.
/// </summary>
public abstract class MutableActor : Actor
{
    [Header("Triggers")]
    [SerializeField][Tooltip("All the triggers that can make execute the action.")]
    private List<Trigger> _triggers;
    public List<Trigger> Triggers { get { return _triggers; } }

    [SerializeField][Tooltip("When the action has been done, all the triggers will stay activated.")]
    private bool _triggersActivatedWhenActionDone;

    [Header("Camera")]
    [SerializeField][Tooltip("The camera that can show the action of the mutable actor when triggered.")]
    protected GenericCamera ActionCamera;

    [Header("Action")]
    [SerializeField][Tooltip("Set the mutable actor's action done or not.")]
    private bool _isActionDone = false;
    public bool IsActionDone { get { return _isActionDone; } }

    [SerializeField][Tooltip("The duration of the action animation.")]
    protected float _actionAnimationDuration = 1f;

    #region Delegates management

    private void OnEnable()
    {
        _triggers.ForEach(trigger => trigger.TriggerActivatedEvent += OnTriggerActivated);
        _triggers.ForEach(trigger => trigger.TriggerDeactivatedEvent += OnTriggerDeactivated);
    }

    private void OnDisable()
    {
        _triggers.ForEach(trigger => trigger.TriggerActivatedEvent -= OnTriggerActivated);
        _triggers.ForEach(trigger => trigger.TriggerDeactivatedEvent -= OnTriggerDeactivated);
    }

    #endregion

    /// <summary>
    /// Deactivate the action camera.
    /// </summary>
    private void Awake() => ActionCamera.gameObject.SetActive(false);

    /// <summary>
    /// Execute the mutable actor's action if all its defined triggers are triggered.
    /// </summary>
    private void OnTriggerActivated()
    {
        if (!_isActionDone && _triggers.TrueForAll(t => t.IsTriggered))
        {
            ExecuteAction();
            _isActionDone = true;
        }
    }

    /// <summary>
    /// Undo the mutable actor's action if it is done.
    /// </summary>
    private void OnTriggerDeactivated()
    {
        if (_isActionDone && !_triggersActivatedWhenActionDone)
        {
            UndoAction();
            _isActionDone = false;
        }
    }

    /// <summary>
    /// Execute the mutable actor's action.
    /// </summary>
    protected abstract void ExecuteAction();

    /// <summary>
    /// Undo the mutable actor's action.
    /// </summary>
    protected abstract void UndoAction();
}
