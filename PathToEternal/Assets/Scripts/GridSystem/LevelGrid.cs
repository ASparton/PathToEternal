using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the level grid, composed of cells.
/// </summary>
public class LevelGrid : MonoBehaviour
{
    static public LevelGrid instance;   // the unique instance of the class.

    #region Attributes

    [Header("Grid level informations")]
    [SerializeField][Tooltip("All the cells of the level grid.")]
    private List<Cell> cells = new List<Cell>();

    [SerializeField][Tooltip("The entry cell of the grid where the player start.")]
    private Cell entryCell = null;

    [SerializeField][Tooltip("The exit cell of the grid that the player has to reach.")]
    private Cell exitCell = null;

    [SerializeField][Tooltip("The player character.")]
    private Player player = null;

    #endregion

    /// <summary>
    /// Assure the validity of the grid level and the singleton pattern.
    /// </summary>
    private void Awake()
    {
        if (entryCell == null)
        {
            print("No entry cell found.");
            return;
        }

        if (exitCell == null)
        {
            print("No exit cell found.");
            return;
        }

        if (player == null)
        {
            print("No player found.");
            return;
        }

        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    #region Delegates management

    /// <summary>
    /// Add needed delegates.
    /// </summary>
    private void OnEnable()
    {
        Player.PlayerMovedEvent += IsLevelCompleted;
    }

    /// <summary>
    /// Remove used delegates.
    /// </summary>
    private void OnDisable()
    {
        Player.PlayerMovedEvent -= IsLevelCompleted;
    }

    #endregion

    /// <summary>
    /// Initialise the grid.
    /// </summary>
    private void Start()
    {
        player.Cell = entryCell;
        Vector3 cellPosition = entryCell.transform.position;
        player.transform.position = new Vector3(cellPosition.x, 0, cellPosition.z);
    }

    /// <summary>
    /// Try to find the cell which has the same position as the one given and returns it.
    /// </summary>
    /// <param name="cellPosition">The position of the cell in the grid.</param>
    /// <returns>The cell that has the corresponding position and null if not found.</returns>
    public Cell GetCell(GridPosition cellPosition)
    {
        foreach (Cell cell in cells)
        {
            if (cell.GridPosition.Equals(cellPosition))
                return cell;
        }
        return null;
    }

    /// <summary>
    /// Called every time the player moved, verify he is on the exit cell to terminate the level.
    /// </summary>
    /// <param name="newPlayerPosition">The new current player cell</param>
    private void IsLevelCompleted(Cell newPlayerCell)
    {
        if (newPlayerCell.GridPosition.Equals(exitCell.GridPosition))
            print("Level completed");
    }
}
