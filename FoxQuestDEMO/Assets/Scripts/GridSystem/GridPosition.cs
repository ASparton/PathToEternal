using System;
using UnityEngine;

/// <summary>
/// Represents a position in a 2D coordinate system.
/// </summary>
public class GridPosition
{
    public readonly int x;
    public readonly int y;

    #region Constructors

    /// <summary>
    /// Instanciate a new position with coordinates equal to 0;0
    /// </summary>
    public GridPosition()
    {
        x = 0;
        y = 0;
    }

    /// <summary>
    /// Instanciate a new position.
    /// </summary>
    /// <param name="x">the position on the x axis (relative to a Grid)</param>
    /// <param name="y">the position on the y axis (relative to a Grid)</param>
    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    #endregion

    /// <summary>
    /// Calculate the distance between this position and the given one.
    /// </summary>
    /// <param name="otherPosition">the other position to calculate the distance to.</param>
    /// <returns>The distance separting this position and the given one.</returns>
    public int DistanceTo(GridPosition otherPosition)
    {
        return Math.Abs(otherPosition.x - x) + Math.Abs(otherPosition.y - y);
    }

    /// <summary>
    /// Determine whether this position is equal to the object given.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>true if the other object is a position that have the exact same coordinates, false otherwise.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != GetType())
            return false;

        if ((GridPosition)obj == this)
            return true;

        GridPosition otherPosition = (GridPosition)obj;
        return otherPosition.x == x && otherPosition.y == y;
    }

    public override int GetHashCode()
    {
        int hashcode = 37;
        hashcode = hashcode * 74 + x.GetHashCode();
        hashcode = hashcode * 74 + y.GetHashCode();
        return hashcode;
    }

    /// <summary>
    /// Converts the position in a string (used for debugging).
    /// </summary>
    /// <returns>"x;y".</returns>
    public override string ToString()
    {
        return x + ";" + y;
    }
}
