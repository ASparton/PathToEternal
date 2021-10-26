using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the level grid, composed of cells.
/// </summary>
public class LevelGrid : MonoBehaviour
{
    static public LevelGrid Instance;   // the unique instance of the class.

    #region Attributes

    [Header("Grid level informations")]
    [SerializeField][Tooltip("All the cells of the level grid.")]
    private List<Cell> cells = new List<Cell>();

    [SerializeField][Tooltip("The entry cell of the grid where the player start.")]
    private Cell entryCell = null;

    [SerializeField][Tooltip("The exit cell of the grid that the player has to reach.")]
    private Cell exitCell = null;

    [SerializeField][Tooltip("The exit portal of the level.")]
    private ExitPortalController exitPortal = null;

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

        if (exitPortal == null)
        {
            print("No exit portal found.");
            return;
        }

        if (player == null)
        {
            print("No player found.");
            return;
        }

        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    #region Delegates management

    /// <summary>
    /// Add needed delegates.
    /// </summary>
    private void OnEnable()
    {
        Player.PlayerMovedEvent += onPlayerMoved;
        ExitPortalController.ExitAnimationFinishedEvent += OnExitAnimationFinished;
    }

    /// <summary>
    /// Remove used delegates.
    /// </summary>
    private void OnDisable()
    {
        Player.PlayerMovedEvent -= onPlayerMoved;
        ExitPortalController.ExitAnimationFinishedEvent -= OnExitAnimationFinished;
    }

    #endregion

    /// <summary>
    /// Initialise the grid.
    /// </summary>
    private void Start()
    {
        player.Cell = entryCell;
        player.Spawn(entryCell.transform.position);
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
    /// Try to find the cell which has the same position as the start position + direction given and returns it.
    /// </summary>
    /// <param name="startPosition">The initial position of a cell.</param>
    /// <param name="direction">The direction to go to from the start position.</param>
    /// <returns>The cell that has the corresponding position and null if not found.</returns>
    public Cell GetCell(GridPosition startPosition, Direction direction)
    {
        return GetCell(GetNextPosition(startPosition, direction));
    }

    /// <summary>
    /// Calculate the next position depending on the start position and the direction given.
    /// </summary>
    /// <param name="startPosition">The initial position.</param>
    /// <param name="direction">The direction to go to from the start position.</param>
    /// <returns>The position of startPosition + direction</returns>
    private static GridPosition GetNextPosition(GridPosition startPosition, Direction direction)
    {
        int x = startPosition.x, y = startPosition.y;
        if (direction == Direction.EAST)
            x++;
        else if (direction == Direction.WEST)
            x--;
        else if (direction == Direction.NORTH)
            y++;
        else
            y--;

        return new GridPosition(x, y);
    }

    /// <summary>
    /// Called every time the player moved, verify he is on the exit cell to terminate the level.
    /// </summary>
    /// <param name="newPlayerPosition">The new current player cell</param>
    private void onPlayerMoved(Cell newPlayerCell)
    {
        if (newPlayerCell.GridPosition.Equals(exitCell.GridPosition))
        {
            exitPortal.StartExitAnimation();
            StartCoroutine(player.PlayerDizzyAnimation(exitPortal.EndAnimationDuration));
            player.SetWeaponsActive(false);
        }
    }

    /// <summary>
    /// Called when the exit portal animation is finished. Make the player disappear and complete the level.
    /// </summary>
    private void OnExitAnimationFinished()
    {
        player.transform.localScale = Vector3.zero;
    }
}
