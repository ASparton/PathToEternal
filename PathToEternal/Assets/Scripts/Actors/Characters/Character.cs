using UnityEngine;

/// <summary>
/// A Character is a DynamicActor that has a life, so can die and disappear from the grid level.
/// </summary>
public class Character : DynamicActor
{
    #region Attributes

    [Header("Life")]

    // Max health
    [SerializeField][Tooltip("The number of damages a Character can receive before he dies.")]
    private int _maxHealth = 5;
    public int MaxHealth { get { return _maxHealth; } }     // The number of damages it can receive before he dies.

    // Current health
    [SerializeField]
    [Tooltip("The current number of damages a Character can receive before he dies.")]
    private int _currentHealth = 5;
    public int Health { get { return _currentHealth; } }    // The current number of a damages it can receive before he dies

    public bool IsDead { get { return _currentHealth <= 0; } }
    #endregion

    /// <summary>
    /// Substract damageAmount from the health of the character. 
    /// </summary>
    /// <param name="damageAmount">The damage amount to substract.</param>
    /// <returns>true if the character died, false otherwise.</returns>
    public bool TakeDamages(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
            return true;
        return false;
    }
}
