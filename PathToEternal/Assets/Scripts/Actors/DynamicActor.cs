public class DynamicActor : Actor
{
    /// <summary>
    /// Change the current actor cell to the cell with the corresponding given position.
    /// </summary>
    /// <param name="position">The position of the cell to set.</param>
    public void SetGridPosition(GridPosition position)
    {
        // Means that it is the same cell, so don't set a new position.
        if (position == Cell.GridPosition)
            return;

        Cell newCell = LevelGrid.instance.GetCell(position);
        if (newCell != null && newCell.Content == null)
        {
            Cell previousCell = Cell;
            Cell = newCell;
            previousCell.Content = null;
        }
    }
}
